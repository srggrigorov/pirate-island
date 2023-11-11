using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SpawnedPooledObject
{
    public GameObject Prefab { get; }
    public bool AutoDespawn { get; }
    public int AutoDespawnDelayMs { get; }
    public CancellationTokenSource CancellationTokenSource { get; private set; }

    private ObjectPooler _objectPooler;

    public SpawnedPooledObject(GameObject prefab, bool autoDespawn, int autoDespawnDelayMs, ObjectPooler objectPooler)
    {
        Prefab = prefab;
        AutoDespawn = autoDespawn;
        AutoDespawnDelayMs = autoDespawnDelayMs;
        _objectPooler = objectPooler;
    }


    public async Task StartAutoDespawn(GameObject gameObject)
    {
        CancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(AutoDespawnDelayMs, CancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (Application.isPlaying)
        {
            _objectPooler.Despawn(gameObject);
        }
    }
}