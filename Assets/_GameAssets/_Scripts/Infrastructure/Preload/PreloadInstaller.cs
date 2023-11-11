using Zenject;

public class PreloadInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AssetsManager>().AsSingle().NonLazy();
    }
}