using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _GameAssets._Scripts.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class EnemySpawner : IDisposable
{
    public event Action<Enemy> OnEnemyAdded;
    public event Action<Enemy> OnEnemyKilled;

    public int CurrentEnemiesCount => _spawnedEnemies.Count;
    public float DespawnDelay => _settings.DespawnDelaySec;

    private EnemySpawnerSettings _settings;
    private Collider _spawnZoneCollider;
    private List<Enemy> _spawnedEnemies;
    private float _spawnDelayCurrent;
    private bool _canSpawnEnemy = true;
    private ObjectPooler _objectPooler;
    private CancellationTokenSource _cancellationTokenSource;
    private SoundManager _soundManager;

    public EnemySpawner(Collider spawnZoneCollider, ObjectPooler objectPooler, AssetsManager assetsManager, SoundManager soundManager)
    {
        _spawnZoneCollider = spawnZoneCollider;
        _spawnedEnemies = new List<Enemy>();
        _objectPooler = objectPooler;
        _soundManager = soundManager;
        _settings = assetsManager.GetModuleSettings<EnemySpawnerSettings>();
        _spawnDelayCurrent = _settings.SpawnStartDelaySec;
    }

    public void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        SetNextSpawn(_cancellationTokenSource.Token).Forget();
    }

    public void StopSpawning() => ClearCancellationToken();

    public void DecreaseSpawnDelay()
    {
        if (_spawnDelayCurrent - _settings.SpawnDelayChangeStepSec <= _settings.SpawnMinDelaySec)
        {
            return;
        }

        _spawnDelayCurrent -= _settings.SpawnDelayChangeStepSec;
    }

    async private UniTask SetNextSpawn(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                await Task.Delay((int)(_spawnDelayCurrent * 1000), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            while (!_canSpawnEnemy)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.Yield();
            }

            SpawnEnemy(_settings.EnemiesPrefabs[Random.Range(0, _settings.EnemiesPrefabs.Count)]);

            if (_settings.PiratesAudioClips != null)
            {
                _soundManager.PlaySoundOnce(_settings.PiratesAudioClips[Random.Range(0, _settings.PiratesAudioClips.Count)]);
            }
        }
    }

    //Made by Sergei Grigorov

    private bool TryGetRandomPointOnNavMesh(out Vector3 randomNavMeshPoint)
    {
        if (_spawnZoneCollider == null)
        {
            randomNavMeshPoint = Vector3.zero;
            return false;
        }
        Bounds spawnBounds = _spawnZoneCollider.bounds;
        randomNavMeshPoint = new Vector3(
            Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            Random.Range(spawnBounds.min.z, spawnBounds.max.z));

        if (NavMesh.SamplePosition(randomNavMeshPoint, out var navMeshHit, 10, 1))
        {
            randomNavMeshPoint = navMeshHit.position;
            return true;
        }

        return false;
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        if (!TryGetRandomPointOnNavMesh(out var randomNavMeshPoint))
        {
            return;
        }

        Enemy enemy = _objectPooler.Spawn(enemyPrefab, randomNavMeshPoint, Quaternion.identity)
            .GetComponent<Enemy>();
        _spawnedEnemies.Add(enemy);
        OnEnemyAdded?.Invoke(enemy);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);
        _objectPooler.Despawn(enemy);
        OnEnemyKilled?.Invoke(enemy);
    }

    private void ClearCancellationToken()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    public void KillAllEnemies()
    {
        var tempEnemiesList = new List<Enemy>(_spawnedEnemies);
        _spawnedEnemies.Clear();

        foreach (var enemy in tempEnemiesList)
        {
            enemy.HandleHealthDeplete();
            OnEnemyKilled?.Invoke(enemy);
        }

        tempEnemiesList.Clear();
    }
    public void Dispose()
    {
        ClearCancellationToken();
    }
}
