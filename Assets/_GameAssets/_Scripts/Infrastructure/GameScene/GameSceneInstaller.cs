using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Spawners Zone Collider")]
    [SerializeField] private Collider _spawnZoneCollider;

    [Space(10), SerializeField]
    private CannonController _cannonController;
    [SerializeField]
    private PlayerStatisticsStorageService _statisticsStorageService;

    [Space(10), Header("Audio sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _vfxSource;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPooler>().AsSingle().NonLazy();
        Container.Bind<InputController>().AsSingle().NonLazy();
        Container.Bind<PlayerStatisticsStorageService>().FromInstance(_statisticsStorageService).AsSingle().NonLazy();
        Container.Bind<ISoundService>().To<SoundService>().AsSingle().WithArguments(_musicSource, _vfxSource).NonLazy();
        Container.Bind<EnemySpawner>().AsSingle().WithArguments(_spawnZoneCollider).NonLazy();
        Container.Bind<GameStateService>().AsSingle().NonLazy();
        Container.Bind<PowerUpSpawner>().AsSingle().WithArguments(_spawnZoneCollider).NonLazy();
        Container.Bind<CannonController>().FromInstance(_cannonController).AsSingle().NonLazy();
        Container.Bind<IDifficultyService>().To<EnemySpawnDelayProgressionService>().AsSingle().NonLazy();
    }
}
