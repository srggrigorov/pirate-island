using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : IMonoBehaviourSpawnerService, IAutoSpawner, IDisposable
{
    public event Action OnSpawned;
    public event Action OnDespawned;

    private readonly PowerUpSpawnerSettings _settings;
    private readonly ObjectPooler _objectPooler;
    private readonly Collider _spawnAreaCollider;
    private readonly GameStateService _gameManager;

    private CancellationTokenSource _cancellationTokenSource;
    private PowerUp _lastSpawnPowerUp;

    public PowerUpSpawner(Collider spawnAreaCollider, ObjectPooler objectPooler, AssetsManager assetsManager, GameStateService gameManager)
    {
        _spawnAreaCollider = spawnAreaCollider;
        _objectPooler = objectPooler;
        _settings = assetsManager.GetModuleSettings<PowerUpSpawnerSettings>();
        _gameManager = gameManager;
        _gameManager.OnGameStarted += StartSpawning;
        _gameManager.OnGameEnded += StopSpawning;
    }

    async private UniTask SetNextSpawn()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await UniTask.Delay((int)(_settings.SpawnDelayTimeSec * 1000), DelayType.DeltaTime,
            PlayerLoopTiming.Update, _cancellationTokenSource.Token).SuppressCancellationThrow();

        if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
        {
            return;
        }

        
        Spawn(_settings.PowerUpPrefabs[Random.Range(0, _settings.PowerUpPrefabs.Count)]);
        ClearCancellationToken();
    }


    public void StartSpawning()
    {
        SetNextSpawn().Forget();
    }

    //Made by Sergei Grigorov

    public void StopSpawning() => ClearCancellationToken();
    public void Spawn(MonoBehaviour prefab)
    {
        _lastSpawnPowerUp = _objectPooler.Spawn(
            prefab, _spawnAreaCollider.RandomPointInside(),
            Quaternion.identity).GetComponent<PowerUp>();

        _lastSpawnPowerUp.OnActivated += StartSpawning;
        OnSpawned?.Invoke();
    }
    public void Despawn(MonoBehaviour instance)
    {
        PowerUp powerUp = instance as PowerUp;
        if (!powerUp)
        {
            return;
        }
        powerUp.OnActivated -= StartSpawning;
        _objectPooler.Despawn(powerUp);
        OnDespawned?.Invoke();
    }
    public void DespawnAll()
    {
        Despawn(_lastSpawnPowerUp);
    }

    private void ClearCancellationToken()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
    public void Dispose()
    {
        ClearCancellationToken();
        if (_gameManager != null)
        {
            _gameManager.OnGameStarted -= StartSpawning;
            _gameManager.OnGameEnded -= StopSpawning;
        }
    }
}
