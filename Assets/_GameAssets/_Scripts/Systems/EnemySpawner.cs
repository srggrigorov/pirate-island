using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance; //Can be changed if Zenject is implemented
    public event Action<Enemy> OnEnemyAdded;
    public event Action<Enemy> OnEnemyKilled;

    public int CurrentEnemiesCount => _spawnedEnemies.Count;

    [SerializeField] private List<Enemy> _enemiesPrefabs;
    [SerializeField] [Min(0)] private float _spawnStartDelaySec;
    [SerializeField] [Min(0)] private float _spawnMinDelaySec;
    [SerializeField] [Min(0)] private float _spawnDelayChangeStepSec;
    [SerializeField] private Renderer _islandRendererNavMesh;

    [Space(5)] [SerializeField] private List<AudioClip> _piratesAudioClips;

    private List<Enemy> _spawnedEnemies;
    private float _spawnDelayCurrent;
    private bool _canSpawnEnemy = true;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        _spawnedEnemies = new List<Enemy>();
        _spawnDelayCurrent = _spawnStartDelaySec;
    }

    public async void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await SetNextSpawn(_cancellationTokenSource.Token);
    }

    public void StopSpawning() => ClearCancellationToken();

    public void DecreaseSpawnDelay()
    {
        if (_spawnDelayCurrent - _spawnDelayChangeStepSec <= _spawnMinDelaySec)
        {
            return;
        }

        _spawnDelayCurrent -= _spawnDelayChangeStepSec;
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
        Bounds rendererBounds = _islandRendererNavMesh.bounds;
        Vector3 randomPoint = new Vector3(Random.Range(rendererBounds.min.x, rendererBounds.max.x),
            rendererBounds.center.y, Random.Range(rendererBounds.min.z, rendererBounds.max.z));

        if (NavMesh.SamplePosition(randomPoint, out var navMeshHit, 4, 1))
        {
            randomNavMeshPoint = navMeshHit.position;
            return true;
        }

        randomNavMeshPoint = randomPoint;
        return false;
    }

    public void SpawnEnemy(Enemy enemyPrefab)
    {
        if (!GetRandomPointOnNavMesh(out var randomNavMeshPoint))
        {
            return;
        }

        Enemy enemy = ObjectPooler.Instance.Spawn(enemyPrefab.gameObject, randomNavMeshPoint, Quaternion.identity)
            .GetComponent<Enemy>();
        _spawnedEnemies.Add(enemy);
        OnEnemyAdded?.Invoke(enemy);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);
        ObjectPooler.Instance.Despawn(enemy.gameObject);
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
            ObjectPooler.Instance.Despawn(enemy.gameObject);
            OnEnemyKilled?.Invoke(enemy);
        }

        tempEnemiesList.Clear();
    }
}