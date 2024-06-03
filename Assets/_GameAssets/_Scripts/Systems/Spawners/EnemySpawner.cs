using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _GameAssets._Scripts.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : IMonoBehaviourSpawnerService, IAutoSpawner, IDisposable
{
    public event Action OnSpawned;
    public event Action OnDespawned;

    public event Action OnEnemyKilled;

    public int CurrentSpawnedCount => _spawnedEnemies.Count;
    public float DespawnDelay => _settings.DespawnDelaySec;
    public int KilledCount { get; private set; }
    public float SpawnDelayCurrent { get; private set; }

    private readonly EnemySpawnerSettings _settings;
    private readonly Collider _spawnZoneCollider;
    private readonly List<Enemy> _spawnedEnemies;
    private bool _canSpawnEnemy = true;
    private readonly ObjectPooler _objectPooler;
    private CancellationTokenSource _cancellationTokenSource;
    private readonly ISoundService _soundService;

    public EnemySpawner(Collider spawnZoneCollider, ObjectPooler objectPooler, AssetsManager assetsManager, ISoundService soundService)
    {
        _spawnZoneCollider = spawnZoneCollider;
        _spawnedEnemies = new List<Enemy>();
        _objectPooler = objectPooler;
        _soundService = soundService;
        _settings = assetsManager.GetModuleSettings<EnemySpawnerSettings>();
        SpawnDelayCurrent = _settings.SpawnStartDelaySec;
    }

    public void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        SetNextSpawn(_cancellationTokenSource.Token).SuppressCancellationThrow().Forget();
    }

    public void StopSpawning() => ClearCancellationToken();

    public void ChangeSpawnDelay(float delta)
    {
        SpawnDelayCurrent = SpawnDelayCurrent + delta > 0 ? SpawnDelayCurrent + delta : 0;
    }

    async private UniTask SetNextSpawn(CancellationToken cancellationToken)
    {
        while (Application.isPlaying)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }


            await UniTask.Delay((int)(SpawnDelayCurrent * 1000), DelayType.DeltaTime,
                    PlayerLoopTiming.Update, cancellationToken, true)
                .SuppressCancellationThrow();

            while (!_canSpawnEnemy)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await UniTask.Yield();
            }

            Spawn(_settings.EnemiesPrefabs[Random.Range(0, _settings.EnemiesPrefabs.Count)]);

            if (_settings.PiratesAudioClips != null)
            {
                _soundService.PlaySoundOnce(_settings.PiratesAudioClips[Random.Range(0, _settings.PiratesAudioClips.Count)]);
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

        randomNavMeshPoint = _spawnZoneCollider.RandomPointInside();

        if (NavMesh.SamplePosition(randomNavMeshPoint, out var navMeshHit, 10, 1))
        {
            randomNavMeshPoint = navMeshHit.position;
            return true;
        }

        return false;
    }

    public void Spawn(MonoBehaviour enemyPrefab)
    {
        if (!TryGetRandomPointOnNavMesh(out var randomNavMeshPoint))
        {
            return;
        }

        Enemy enemy = _objectPooler.Spawn(enemyPrefab, randomNavMeshPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.OnKilled += HandleEnemyKill;
        _spawnedEnemies.Add(enemy);
        OnSpawned?.Invoke();
    }

    private void HandleEnemyKill(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);
        KilledCount++;
        OnEnemyKilled?.Invoke();
        enemy.OnKilled -= HandleEnemyKill;
    }

    public void Despawn(MonoBehaviour enemyInstance)
    {
        if (_spawnedEnemies.Contains((Enemy)enemyInstance))
        {
            _spawnedEnemies.Remove((Enemy)enemyInstance);
        }
        _objectPooler.Despawn(enemyInstance);
        OnDespawned?.Invoke();
    }

    private void ClearCancellationToken()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    public void DespawnAll()
    {
        ReadOnlySpan<Enemy> enemiesSpan = _spawnedEnemies.ToArray();

        foreach (var enemy in enemiesSpan)
        {
            Despawn(enemy);
        }
    }

    public void KillAll()
    {
        ReadOnlySpan<Enemy> enemiesSpan = _spawnedEnemies.ToArray();

        foreach (var enemy in enemiesSpan)
        {
            enemy.HandleHealthDeplete();
        }
    }
    public void Dispose() => ClearCancellationToken();
}
