using TMPro;
using UnityEngine;

public class PlaymodeStatistics : MonoBehaviour
{
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _enemiesCountText;

    private void OnEnable()
    {
        EnemySpawner.Instance.OnEnemyKilled += UpdateInfo;
        EnemySpawner.Instance.OnEnemyAdded += UpdateInfo;
        UpdateInfo(null);
    }

    private void OnDisable()
    {
        EnemySpawner.Instance.OnEnemyKilled -= UpdateInfo;
        EnemySpawner.Instance.OnEnemyAdded -= UpdateInfo;
    }

    private void UpdateInfo(Enemy obj)
    {
        _killsText.text = GameManager.Instance.EnemiesKilled.ToString();
        _enemiesCountText.text =
            $"{EnemySpawner.Instance.CurrentEnemiesCount} / {GameManager.Instance.EnemyCountForDefeat}";
    }
}