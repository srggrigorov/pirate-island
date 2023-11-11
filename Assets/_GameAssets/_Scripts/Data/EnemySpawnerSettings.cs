using UnityEngine;

namespace _GameAssets._Scripts.Data
{
    [CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "Settings/Enemy Spawner", order = 0)]
    public class EnemySpawnerSettings : ModuleSettings
    {
        [Min(0)] public float SpawnStartDelaySec;
        [Min(0)] public float SpawnMinDelaySec;
        [Min(0)] public float SpawnDelayChangeStepSec;
    }
}