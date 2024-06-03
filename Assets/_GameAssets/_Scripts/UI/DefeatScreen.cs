using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _enemiesKilled;
    [FormerlySerializedAs("_newRecord")]
    [SerializeField] private GameObject _newHighScore;

    private ZenjectSceneLoader _sceneLoader;
    private AssetsManager _assetsManager;
    private GameStateService _gameManager;

    [Inject]
    private void Construct(ZenjectSceneLoader sceneLoader, AssetsManager assetsManager, GameStateService gameManager)
    {
        _sceneLoader = sceneLoader;
        _assetsManager = assetsManager;
        _gameManager = gameManager;
        _gameManager.OnGameEnded += () => gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(() => _sceneLoader.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex,
            LoadSceneMode.Single,
            container => { container.Bind<AssetsManager>().FromInstance(_assetsManager).AsSingle().NonLazy(); }));

        _enemiesKilled.text = $"{_gameManager.EnemiesKilled}";
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.HighScore.ToString(), 0) < _gameManager.EnemiesKilled)
        {
            _newHighScore.SetActive(true);
            PlayerPrefs.SetInt(PlayerPrefsKeys.HighScore.ToString(), _gameManager.EnemiesKilled);
        }
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveAllListeners();
    }
}
