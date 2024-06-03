using TMPro;
using UnityEngine;
using Zenject;

public class PlaymodeStatisticsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _enemiesCountText;

    private EnemySpawner _enemySpawner;
    private GameStateService _gameManager;

    [Inject]
    private void Construct(EnemySpawner enemySpawner, GameStateService gameManager)
    {
        _enemySpawner = enemySpawner;
        _gameManager = gameManager;
        _gameManager.OnGameStarted += () => gameObject.SetActive(true);
        _gameManager.OnGameEnded += () => gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _enemySpawner.OnSpawned += UpdateInfo;
        _enemySpawner.OnEnemyKilled += UpdateInfo;
        UpdateInfo();
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= UpdateInfo;
        _enemySpawner.OnSpawned -= UpdateInfo;
    }

    private void UpdateInfo()
    {
        _killsText.text = _gameManager.EnemiesKilled.ToString();
        _enemiesCountText.text =
            $"{_enemySpawner.CurrentSpawnedCount} / {_gameManager.EnemiesCountForDefeat}";
    }
}
