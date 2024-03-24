using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class PowerUpKillAll : PowerUp
{
    [Inject]
    private void Construct(EnemySpawner enemySpawner)
    {
        PowerUpEffect = new PowerUpEffectKillAll(enemySpawner, this);
    }

    public override void Despawn()
    {
        _objectPooler.Despawn(this);
    }
}