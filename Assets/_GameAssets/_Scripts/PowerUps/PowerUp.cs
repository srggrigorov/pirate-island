using System;
using UnityEngine;
using Zenject;

public abstract class PowerUp : MonoBehaviour
{
    public Action OnActivated;
    public abstract void Activate();

    protected ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }
}