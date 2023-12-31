using System.Collections.Generic;
using _GameAssets._Scripts.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolerSettings", menuName = "Settings/Object Pooler", order = 0)]
public class ObjectPoolerSettings : ModuleSettings
{
    public List<PoolData> _poolsDataList;
}