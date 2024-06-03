using System;

class EnemySpawnDelayProgressionService : IDifficultyService, IDisposable
{
    private readonly EnemySpawner _enemySpawnService;
    private readonly EnemySpawnDelayProgressionSettings _settings;

    public EnemySpawnDelayProgressionService(EnemySpawner enemySpawnService, AssetsManager assetsManager)
    {
        _enemySpawnService = enemySpawnService;
        _enemySpawnService.OnEnemyKilled += HandleEnemyKill;
        _settings = assetsManager.GetModuleSettings<EnemySpawnDelayProgressionSettings>();
    }

    private void HandleEnemyKill()
    {
        if (_enemySpawnService.KilledCount % _settings.DecreaseOnEveryNKilledEnemy == 0)
        {
            IncreaseDifficulty();
        }
    }

    public void IncreaseDifficulty()
    {
        if (_enemySpawnService.SpawnDelayCurrent - _settings.EnemySpawnDelayChangeStep >= _settings.MinEnemySpawnDelay)
        {
            _enemySpawnService.ChangeSpawnDelay(-_settings.EnemySpawnDelayChangeStep);
        }
    }
    public void DecreaseDifficulty()
    {
        _enemySpawnService.ChangeSpawnDelay(_settings.EnemySpawnDelayChangeStep);
    }
    public void Dispose()
    {
        _enemySpawnService.OnEnemyKilled -= HandleEnemyKill;
    }
}
