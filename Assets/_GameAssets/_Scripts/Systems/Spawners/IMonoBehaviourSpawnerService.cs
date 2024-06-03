using System;
using UnityEngine;

public interface IMonoBehaviourSpawnerService
{
    event Action OnSpawned;
    event Action OnDespawned;
    
    void Spawn(MonoBehaviour prefab);
    void Despawn(MonoBehaviour instance);
    void DespawnAll();
}