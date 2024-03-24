using System;
using UnityEngine;
using Zenject;

public abstract class PowerUp : MonoBehaviour
{
    public Action OnActivated;
    public IPowerUpEffect PowerUpEffect { get; protected set; }

    protected ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }

    public abstract void Despawn();
}