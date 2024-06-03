using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerStatisticsScreen : MenuOptionScreen
{
    [FormerlySerializedAs("_recordText")]
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private TMP_Text _totalEnemiesKilledText;
    [SerializeField] private TMP_Text _shotsFired;

    private PlayerStatisticsStorageService _statisticsStorageService;

    [Inject]
    private void Construct(PlayerStatisticsStorageService statisticsStorageService)
    {
        _statisticsStorageService = statisticsStorageService;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _highScoreText.text = _statisticsStorageService.StatisticsData.HighScore.ToString();
        _totalEnemiesKilledText.text = _statisticsStorageService.StatisticsData.TotalEnemiesKilled.ToString();
        _shotsFired.text = _statisticsStorageService.StatisticsData.ShotsFired.ToString();
    }
}
