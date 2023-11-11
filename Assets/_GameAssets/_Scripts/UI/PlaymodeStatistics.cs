using TMPro;
using UnityEngine;
using Zenject;

public class PlaymodeStatistics : MonoBehaviour
{
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _enemiesCountText;

    private EnemySpawner _enemySpawner;
    private GameManager _gameManager;

    [Inject]
    private void Construct(EnemySpawner enemySpawner, GameManager gameManager)
    {
        _enemySpawner = enemySpawner;
        _gameManager = gameManager;
    }

    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += UpdateInfo;
        _enemySpawner.OnEnemyAdded += UpdateInfo;
        UpdateInfo(null);
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= UpdateInfo;
        _enemySpawner.OnEnemyAdded -= UpdateInfo;
    }

    private void UpdateInfo(Enemy enemy)
    {
        _killsText.text = _gameManager.EnemiesKilled.ToString();
        _enemiesCountText.text =
            $"{_enemySpawner.CurrentEnemiesCount} / {_gameManager.EnemyCountForDefeat}";
    }
}