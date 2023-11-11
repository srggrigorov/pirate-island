using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class PowerUpKillAll : PowerUp
{
    private EnemySpawner _enemySpawner;

    [Inject]
    private void Construct(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
    }

    public override void Activate()
    {
        _enemySpawner.DespawnAllEnemies();
        OnActivated?.Invoke();
        OnActivated = null;
        _objectPooler.Despawn(gameObject);
    }
}