using UnityEngine;

[DisallowMultipleComponent]
public class PooledParticleSystem : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPooler.Instance.Despawn(gameObject);
    }
}