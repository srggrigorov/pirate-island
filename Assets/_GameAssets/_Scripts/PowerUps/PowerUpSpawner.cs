using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : IDisposable
{
    private PowerUpSpawnerSettings _settings;
    private ObjectPooler _objectPooler;
    private readonly Collider _spawnAreaCollider;
    
    private CancellationTokenSource _cancellationTokenSource;
    private PowerUp _lastSpawnPowerUp;
    
    public PowerUpSpawner(Collider spawnAreaCollider, ObjectPooler objectPooler, AssetsManager assetsManager)
    {
        _spawnAreaCollider = spawnAreaCollider;
        _objectPooler = objectPooler;
        _settings = assetsManager.GetModuleSettings<PowerUpSpawnerSettings>();
    }

    async private UniTask SetNextSpawn()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            await UniTask.Delay((int)(_settings.SpawnDelayTimeSec * 1000), DelayType.DeltaTime, 
                                PlayerLoopTiming.Update, _cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        Bounds spawnBounds = _spawnAreaCollider.bounds;
        Vector3 _randomPointInSpawnArea = new Vector3
        {
            x = Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            y = Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            z = Random.Range(spawnBounds.min.z, spawnBounds.max.z)
        };
        _lastSpawnPowerUp = _objectPooler.Spawn(
            _settings.PowerUpPrefabs[Random.Range(0, _settings.PowerUpPrefabs.Count)], _randomPointInSpawnArea,
            Quaternion.identity).GetComponent<PowerUp>();
        _lastSpawnPowerUp.OnActivated += StartSpawning;
    }

    public void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        SetNextSpawn().Forget();
    }

    //Made by Sergei Grigorov

    public void StopSpawning() => ClearCancellationToken();

    private void ClearCancellationToken()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
    public void Dispose()
    {
        ClearCancellationToken();
    }
}
