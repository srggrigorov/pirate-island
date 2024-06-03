using System;
using System.Collections.Generic;
using _GameAssets._Scripts.Settings;
using UnityEngine;
using UnityEngine.Analytics;

public class GameStateService : IDisposable
{
    public event Action OnGameStarted;
    public event Action OnGameEnded;
    public int EnemiesKilled { get; private set; }
    public int EnemiesCountForDefeat => _settings.EnemiesCountForDefeat;

    private readonly EnemySpawner _enemySpawner;
    private readonly DefeatSettings _settings;
    private readonly PlayerStatisticsStorageService _statisticsStorageService;
    private float _startTime;

    public GameStateService(EnemySpawner enemySpawner, AssetsManager assetsManager, PlayerStatisticsStorageService statisticsStorageService)
    {
        _enemySpawner = enemySpawner;
        _settings = assetsManager.GetModuleSettings<DefeatSettings>();
        _statisticsStorageService = statisticsStorageService;
    }

    private void HandleEnemyKill()
    {
        EnemiesKilled++;
        _statisticsStorageService.StatisticsData.TotalEnemiesKilled++;
        _statisticsStorageService.CheckForHighScore(EnemiesKilled);
    }


    private void CheckForDefeat()
    {
        if (_enemySpawner.CurrentSpawnedCount < EnemiesCountForDefeat)
        {
            return;
        }

        _enemySpawner.OnSpawned -= CheckForDefeat;
        OnGameEnded?.Invoke();
        _enemySpawner.StopSpawning();
        _statisticsStorageService.SaveStatistics();
        Analytics.CustomEvent("Defeat", new Dictionary<string, object>
        {
            { "Kills", _enemySpawner.KilledCount },
            { "Time in game", Time.time - _startTime }
        });
    }

    public void StartGame()
    {
        _enemySpawner.OnSpawned += CheckForDefeat;
        _enemySpawner.OnEnemyKilled += HandleEnemyKill;
        _enemySpawner.StartSpawning();
        _startTime = Time.time;
        OnGameStarted?.Invoke();
    }
    public void Dispose()
    {
        if (_enemySpawner != null)
        {
            _enemySpawner.OnSpawned -= CheckForDefeat;
            _enemySpawner.OnEnemyKilled -= HandleEnemyKill;
        }
    }
}
