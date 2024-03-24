using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class PooledParticleSystem : MonoBehaviour
{
    private ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }

    private void OnParticleSystemStopped()
    {
        _objectPooler.Despawn(this);
    }
}