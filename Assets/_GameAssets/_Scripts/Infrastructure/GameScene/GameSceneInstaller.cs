using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Collider _spawnZoneCollider;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CannonController _cannonController;
    
    [Space(10), Header("Audio sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _vfxSource;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPooler>().AsSingle().NonLazy();
        Container.Bind<InputController>().AsSingle().NonLazy();
        Container.Bind<SoundManager>().AsSingle().WithArguments(_musicSource, _vfxSource).NonLazy();
        Container.Bind<EnemySpawner>().AsSingle().WithArguments(_spawnZoneCollider).NonLazy();
        Container.Bind<PowerUpSpawner>().AsSingle().WithArguments(_spawnZoneCollider).NonLazy();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
        Container.Bind<CannonController>().FromInstance(_cannonController).AsSingle().NonLazy();
    }
}
