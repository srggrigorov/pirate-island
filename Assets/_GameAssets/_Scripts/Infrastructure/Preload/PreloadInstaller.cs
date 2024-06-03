using UnityEngine;
using Zenject;

public class PreloadInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AssetsManager>().AsSingle().NonLazy();
#if PLATFORM_ANDROID && !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}
