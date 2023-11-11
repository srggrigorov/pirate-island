using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] [Min(0)] private float _spawnDelayTimeSec;
    [SerializeField] private Collider _spawnAreaCollider;
    [SerializeField] private List<PowerUp> _powerUpPrefabs;

    private CancellationTokenSource _cancellationTokenSource;
    private PowerUp _lastSpawnPowerUp;
    private ObjectPooler _objectPooler;

    [Inject]
    public void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }

    private async Task SetNextSpawn()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            await Task.Delay((int)(_spawnDelayTimeSec * 1000), _cancellationTokenSource.Token);
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
            _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Count)].gameObject, _randomPointInSpawnArea,
            Quaternion.identity).GetComponent<PowerUp>();
        _lastSpawnPowerUp.OnActivated += StartSpawning;
    }

    public async void StartSpawning()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await SetNextSpawn();
    }

    //Made by Sergei Grigorov

    public void StopSpawning() => ClearCancellationToken();

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
}