using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _enemyCountForDefeat;
    public int EnemyCountForDefeat => _enemyCountForDefeat;

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private PowerUpSpawner _powerUpSpawner;
    [SerializeField] private CannonController _cannonController;
    [SerializeField] private TransformRotator _shipRotator;

    [Space(5), SerializeField] private DefeatScreen _defeatScreen;
    [SerializeField] private PlaymodeStatistics _playmodeStatistics;

    [Header("Difficulty progression")]
    [SerializeField]
    [Tooltip(
        "Decreases spawn delay when every n-enemy is killed. If FALSE, spawn delay will decrease according to list SpawnDelayDecreaseKillCount")]
    private bool _decreaseSpawnDelayOnEveryNEnemy;

    [SerializeField] private int _nEnemy;
    [Space(10), SerializeField] private List<int> _spawnDelayDecreaseKillsCounts = new List<int>();

    public int EnemiesKilled { get; private set; }

    private void Start()
    {
        _enemySpawner.OnEnemyAdded += CheckForDefeat;
        _enemySpawner.OnEnemyKilled += HandleEnemyKill;
    }

    private void HandleEnemyKill(Enemy enemy)
    {
        EnemiesKilled++;

        //Player Prefs is used to speed up development process, player data and statistics should be saved in a JSON
        PlayerPrefs.SetInt(PlayerPrefsKeys.TotalEnemiesKilled.ToString(),
            PlayerPrefs.GetInt(PlayerPrefsKeys.TotalEnemiesKilled.ToString(), 0) + 1);

        if (_decreaseSpawnDelayOnEveryNEnemy)
        {
            if (EnemiesKilled % _nEnemy == 0)
            {
                _enemySpawner.DecreaseSpawnDelay();
            }
        }
        else if (_spawnDelayDecreaseKillsCounts.Contains(EnemiesKilled))
        {
            _enemySpawner.DecreaseSpawnDelay();
        }
    }

    private void CheckForDefeat(Enemy obj)
    {
        if (_enemySpawner.CurrentEnemiesCount >= _enemyCountForDefeat)
        {
            Defeat();
        }
    }

    public void StartGame()
    {
        _enemySpawner.StartSpawning();
        _powerUpSpawner.StartSpawning();
        _shipRotator.enabled = _cannonController.CanShoot = true;
        _playmodeStatistics.gameObject.SetActive(true);
    }

    private void Defeat()
    {
        _enemySpawner.OnEnemyAdded -= CheckForDefeat;
        _enemySpawner.StopSpawning();
        _powerUpSpawner.StopSpawning();
        _cannonController.CanShoot = false;
        _defeatScreen.gameObject.SetActive(true);
        _playmodeStatistics.gameObject.SetActive(false);
    }
}