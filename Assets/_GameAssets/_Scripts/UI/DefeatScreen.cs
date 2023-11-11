using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _enemiesKilled;
    [SerializeField] private GameObject _newRecord;

    private ZenjectSceneLoader _sceneLoader;
    private AssetsManager _assetsManager;
    private GameManager _gameManager;

    [Inject]
    private void Construct(ZenjectSceneLoader sceneLoader, AssetsManager assetsManager, GameManager gameManager)
    {
        _sceneLoader = sceneLoader;
        _assetsManager = assetsManager;
        _gameManager = gameManager;
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(() => _sceneLoader.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex,
            LoadSceneMode.Single,
            container => { container.Bind<AssetsManager>().FromInstance(_assetsManager).AsSingle().NonLazy(); }));

        _enemiesKilled.text = $"{_gameManager.EnemiesKilled}";
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.Record.ToString(), 0) < _gameManager.EnemiesKilled)
        {
            _newRecord.SetActive(true);
            PlayerPrefs.SetInt(PlayerPrefsKeys.Record.ToString(), _gameManager.EnemiesKilled);
        }
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveAllListeners();
    }
}