using System.Collections.Generic;
using _GameAssets._Scripts.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSpawnerSettings", menuName = "Settings/PowerUp Spawner")]
public class PowerUpSpawnerSettings : ModuleSettings
{
    [field: SerializeField] [field: Min(0)]
    public float SpawnDelayTimeSec { get; private set; }
    [field: SerializeField] 
    public List<PowerUp> PowerUpPrefabs { get; private set; }
}
