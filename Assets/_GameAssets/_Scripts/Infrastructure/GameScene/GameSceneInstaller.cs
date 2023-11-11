using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private GameManager _gameManager;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPooler>().AsSingle().NonLazy();
        Container.Bind<InputController>().AsSingle().NonLazy();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
        Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsSingle().NonLazy();
    }
}