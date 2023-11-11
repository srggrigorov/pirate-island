using UnityEngine;
using Zenject;

public class GameScene : MonoBehaviour
{
    private ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }

    private void Awake()
    {
        _objectPooler.RegisterPrefabs();
    }
}