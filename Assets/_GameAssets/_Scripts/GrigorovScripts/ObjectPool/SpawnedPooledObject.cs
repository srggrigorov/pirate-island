using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnedPooledObject
{
    public GameObject Prefab { get; }
    public bool AutoDespawn { get; }
    public int AutoDespawnDelayMs { get; }
    public CancellationTokenSource CancellationTokenSource { get; private set; }

    public SpawnedPooledObject(GameObject prefab, bool autoDespawn, int autoDespawnDelayMs)
    {
        Prefab = prefab;
        AutoDespawn = autoDespawn;
        AutoDespawnDelayMs = autoDespawnDelayMs;
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
            ObjectPooler.Instance.Despawn(gameObject);
        }
    }
}