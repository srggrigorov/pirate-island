using _GameAssets._Scripts.Settings;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnDelayProgressionSettings", menuName = "Settings/Difficulty/Enemy Spawn Delay Progression", order = 0)]
public class EnemySpawnDelayProgressionSettings : ModuleSettings
{
    [field: SerializeField, Min(0)]
    public float EnemySpawnDelayChangeStep { get; private set; }
    [field: SerializeField, Min(0)]
    public float MinEnemySpawnDelay { get; private set; }
    [field: SerializeField, Min(0)]
    public int DecreaseOnEveryNKilledEnemy { get; private set; }
}
