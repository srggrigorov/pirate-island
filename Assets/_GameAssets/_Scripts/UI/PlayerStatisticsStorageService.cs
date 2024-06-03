using System.IO;
using _GameAssets._Scripts;
using Cysharp.Threading.Tasks.Triggers;
using UnityEditor;
using UnityEngine;

public class PlayerStatisticsStorageService : MonoBehaviour //MonoBehaviour is used for OnApplicationFocus method
{
    private AsyncApplicationFocusTrigger _focusTrigger;
    private const string KEY = "PLayerStatistics";
    public PlayerStatisticsData StatisticsData { get; private set; }
    private IStorageService _storageService;

    public void Awake()
    {
        _storageService = new BinaryStorageService();
        if (File.Exists(Path.Combine(Application.persistentDataPath, KEY)))
        {
            _storageService.Load<PlayerStatisticsData>(KEY, data => StatisticsData = data);
        }
        else
        {
            StatisticsData = new PlayerStatisticsData();
        }
    }

    public void CheckForHighScore(int currentScore)
    {
        if (currentScore > StatisticsData.HighScore)
        {
            StatisticsData.HighScore = currentScore;
        }
    }


    public void SaveStatistics() => _storageService.Save(KEY, StatisticsData);

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveStatistics();
        }
    }

#if UNITY_EDITOR
    [MenuItem("Edit/Clear Statistics")]
    public static void ClearStatistics()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, KEY)))
        {
            Debug.Log("Statistics have already been cleared!");
            return;
        }
        File.Delete(Path.Combine(Application.persistentDataPath, KEY));
        Debug.Log("Statistics cleared!");
    }
#endif
}
