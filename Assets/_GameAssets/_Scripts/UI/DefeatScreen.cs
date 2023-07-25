using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _enemiesKilled;
    [SerializeField] private GameObject _newRecord;

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        _enemiesKilled.text = $"{GameManager.Instance.EnemiesKilled}";
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.Record.ToString(), 0) < GameManager.Instance.EnemiesKilled)
        {
            _newRecord.SetActive(true);
            PlayerPrefs.SetInt(PlayerPrefsKeys.Record.ToString(), GameManager.Instance.EnemiesKilled);
        }
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveAllListeners();
    }
}