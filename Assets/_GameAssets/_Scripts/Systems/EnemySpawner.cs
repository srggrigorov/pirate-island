using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _GameAssets._Scripts.Data;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    public event Action<Enemy> OnEnemyAdded;
    public event Action<Enemy> OnEnemyKilled;

    public int CurrentEnemiesCount => _spawnedEnemies.Count;

    [SerializeField] private List<Enemy> _enemiesPrefabs;

    [SerializeField] private Transform _navMeshSurfaceTransform;

    [Space(5)] [SerializeField] private List<AudioClip> _piratesAudioClips;

    private List<Enemy> _spawnedEnemies;
    private float _spawnDelayCurrent;
    private bool _canSpawnEnemy = true;
    private ObjectPooler _objectPooler;
    private CancellationTokenSource _cancellationTokenSource;
    private EnemySpawnerSettings _settings;

    [Inject]
    private void Construct(ObjectPooler objectPooler, AssetsManager assetsManager)
    {
        _objectPooler = objectPooler;
        _settings = assetsManager.GetModuleSettings<EnemySpawnerSettings>();
    }

    private void Awake()
    {
        _spawnedEnemies = new List<Enemy>();
        _spawnDelayCurrent = _settings.SpawnStartDelaySec;
    }

    public async void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await SetNextSpawn(_cancellationTokenSource.Token);
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

    private async Task SetNextSpawn(CancellationToken cancellationToken)
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

            SpawnEnemy(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Count)]);

            if (_piratesAudioClips != null)
            {
                SoundManager.Instance.PlaySoundOnce(_piratesAudioClips[Random.Range(0, _piratesAudioClips.Count)]);
            }
        }
    }

    //Made by Sergei Grigorov

    public bool GetRandomPointOnNavMesh(out Vector3 randomNavMeshPoint)
    {
        if (NavMesh.SamplePosition(_navMeshSurfaceTransform.position, out var navMeshHit, 100, 1))
        {
            randomNavMeshPoint = navMeshHit.position;
            return true;
        }

        randomNavMeshPoint = _navMeshSurfaceTransform.position;
        return false;
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        if (!GetRandomPointOnNavMesh(out var randomNavMeshPoint))
        {
            return;
        }

        Enemy enemy = _objectPooler.Spawn(enemyPrefab.gameObject, randomNavMeshPoint, Quaternion.identity)
            .GetComponent<Enemy>();
        _spawnedEnemies.Add(enemy);
        OnEnemyAdded?.Invoke(enemy);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);
        _objectPooler.Despawn(enemy.gameObject);
        OnEnemyKilled?.Invoke(enemy);
    }

    private void OnDestroy()
    {
        ClearCancellationToken();
    }

    private void ClearCancellationToken()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    public void DespawnAllEnemies()
    {
        var tempEnemiesList = new List<Enemy>(_spawnedEnemies);
        _spawnedEnemies.Clear();

        foreach (var enemy in tempEnemiesList)
        {
            _objectPooler.Despawn(enemy.gameObject);
            OnEnemyKilled?.Invoke(enemy);
        }

        tempEnemiesList.Clear();
    }
}