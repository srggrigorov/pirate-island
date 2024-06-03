using System.Collections.Generic;
using _GameAssets._Scripts.Settings;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolerSettings", menuName = "Settings/Object Pooler", order = 0)]
public class ObjectPoolerSettings : ModuleSettings
{
    [field: SerializeField]
    public List<PoolData> PoolsDataList { get; private set; }
}