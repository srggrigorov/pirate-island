using UnityEngine;

[DisallowMultipleComponent]
public class PowerUpKillAll : PowerUp
{
    public override void Activate()
    {
        EnemySpawner.Instance.DespawnAllEnemies();
        OnActivated?.Invoke();
        OnActivated = null;
        ObjectPooler.Instance.Despawn(gameObject);
    }
}