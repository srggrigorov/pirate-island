using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Preload : MonoBehaviour
{
    [SerializeField] private string _gameSceneName;
    private AssetsManager _assetsManager;
    private ZenjectSceneLoader _sceneLoader;

    [Inject]
    private void Construct(AssetsManager assetsManager, ZenjectSceneLoader sceneLoader)
    {
        _assetsManager = assetsManager;
        _sceneLoader = sceneLoader;
    }

    private async void Awake()
    {
        await _assetsManager.InitializeAsync();
        await _assetsManager.LoadModulesSettings();
        _sceneLoader.LoadSceneAsync(_gameSceneName, LoadSceneMode.Single,
            container => { container.Bind<AssetsManager>().FromInstance(_assetsManager).AsSingle().NonLazy(); });
    }
}