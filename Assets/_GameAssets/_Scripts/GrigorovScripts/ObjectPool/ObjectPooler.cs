using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

[DisallowMultipleComponent]
public class ObjectPooler : IDisposable
{
    private ObjectPoolerSettings _settings;
    private DiContainer _container;

    private Dictionary<GameObject, ObjectPool<GameObject>> _objectPools = new();

    private Dictionary<int, SpawnedPooledObject> _spawnedObjects = new();


    [Inject]
    public ObjectPooler(AssetsManager assetsManager, DiContainer container)
    {
        _container = container;
        _settings = assetsManager.GetModuleSettings<ObjectPoolerSettings>();
    }

    public void RegisterPrefabs()
    {
        foreach (var poolData in _settings._poolsDataList)
        {
            RegisterPrefabInternal(poolData.Prefab, poolData.PrewarmCount, poolData.AutoDespawn,
                poolData.AutoDespawnDelaySec);
        }
    }

    private void RegisterPrefabInternal(GameObject prefab, int prewarmCount, bool autoDespawn,
        float autoDespawnDelaySec)
    {
        GameObject CreateFunc()
        {
            var createdObject = _container.InstantiatePrefab(prefab);
            _spawnedObjects[createdObject.GetHashCode()] = new SpawnedPooledObject
            (
                prefab,
                autoDespawn,
                (int)(autoDespawnDelaySec * 1000),
                this
            );
            return createdObject;
        }

        async void ActionOnGet(GameObject pooledObject)
        {
            SpawnedPooledObject spawnedPooledObject = _spawnedObjects[pooledObject.GetHashCode()];
            pooledObject.SetActive(true);
            if (spawnedPooledObject.AutoDespawn)
            {
                await spawnedPooledObject.StartAutoDespawn(pooledObject);
            }
        }

        void ActionOnRelease(GameObject pooledObject)
        {
            SpawnedPooledObject spawnedPooledObject = _spawnedObjects[pooledObject.GetHashCode()];
            pooledObject.SetActive(false);
            spawnedPooledObject.CancellationTokenSource?.Cancel();
            spawnedPooledObject.CancellationTokenSource?.Dispose();
        }

        void ActionOnDestroy(GameObject pooledObject)
        {
            int pooledObjectHashCode = pooledObject.GetHashCode();
            _spawnedObjects[pooledObjectHashCode].CancellationTokenSource?.Cancel();
            _spawnedObjects[pooledObjectHashCode].CancellationTokenSource?.Dispose();
            _spawnedObjects.Remove(pooledObjectHashCode);
            Object.Destroy(pooledObject);
        }

        _objectPools[prefab] = new ObjectPool<GameObject>(
            CreateFunc, ActionOnGet, ActionOnRelease,
            ActionOnDestroy, defaultCapacity: prewarmCount
        );

        var prewarmGameObjects = new List<GameObject>();
        for (var i = 0; i < prewarmCount; i++)
        {
            prewarmGameObjects.Add(_objectPools[prefab].Get());
        }

        foreach (var prewarmObject in prewarmGameObjects)
        {
            _objectPools[prefab].Release(prewarmObject);
        }
    }

    public GameObject Spawn(GameObject prefab)
    {
        if (_objectPools.ContainsKey(prefab))
        {
            return _objectPools[prefab].Get();
        }

        Debug.LogError($"No Object Pool for prefab {prefab.name}!");
        return null;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var spawnedObject = Spawn(prefab);
        if (spawnedObject == null)
        {
            return null;
        }

        spawnedObject.transform.SetPositionAndRotation(position, rotation);
        spawnedObject.transform.SetParent(parent);
        return spawnedObject;
    }

    public void Despawn(GameObject prefabInstance)
    {
        var hashCode = prefabInstance.GetHashCode();
        if (!IsPoolable(hashCode))
        {
            Debug.LogError($"GameObject {prefabInstance.name} is not poolable!");
            return;
        }

        if (prefabInstance.activeInHierarchy == false)
        {
            Debug.LogWarning($"GameObject {prefabInstance.name} is already in pool!");
            return;
        }

        _objectPools[_spawnedObjects[hashCode].Prefab].Release(prefabInstance);
    }

    public bool IsPoolable(GameObject prefabInstance) => IsPoolable(prefabInstance.GetHashCode());

    private bool IsPoolable(int hashCode)
    {
        return _spawnedObjects.ContainsKey(hashCode);
    }

    public void Dispose()
    {
        _objectPools.Clear();
        _spawnedObjects.Clear();
    }
}