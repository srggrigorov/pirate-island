public class PowerUpEffectKillAll : IPowerUpEffect
{
    private EnemySpawner _enemySpawner;
    private PowerUp _powerUp;

    public PowerUpEffectKillAll(EnemySpawner enemySpawner, PowerUp powerUp)
    {
        _enemySpawner = enemySpawner;
        _powerUp = powerUp;
    }

    public void Activate()
    {
        _enemySpawner.KillAll();
        _powerUp.OnActivated?.Invoke();
        _powerUp.OnActivated = null;
        _powerUp.Despawn();
    }
}
