using TMPro;
using UnityEngine;

public class StatisticsScreen : MenuOptionScreen
{
    [SerializeField] private TMP_Text _recordText;
    [SerializeField] private TMP_Text _totalEnemiesKilledText;
    [SerializeField] private TMP_Text _shotsFired;

    protected override void OnEnable()
    {
        base.OnEnable();
        _recordText.text = $"{PlayerPrefs.GetInt(PlayerPrefsKeys.Record.ToString(), 0)}";
        _totalEnemiesKilledText.text = $"{PlayerPrefs.GetInt(PlayerPrefsKeys.TotalEnemiesKilled.ToString(), 0)}";
        _shotsFired.text = $"{PlayerPrefs.GetInt(PlayerPrefsKeys.ShotsFired.ToString(), 0)}";
    }
}