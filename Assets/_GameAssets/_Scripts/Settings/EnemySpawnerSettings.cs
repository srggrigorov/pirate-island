using System.Collections.Generic;
using UnityEngine;

namespace _GameAssets._Scripts.Data
{
    [CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "Settings/Enemy Spawner", order = 0)]
    public class EnemySpawnerSettings : ModuleSettings
    {
        [field: SerializeField]
        public List<Enemy> EnemiesPrefabs { get; private set; }
        [field: SerializeField]
        public List<AudioClip> PiratesAudioClips { get; private set; }

        [field: Space(10)]
        [field: SerializeField] [field: Min(0)]
        public float SpawnStartDelaySec { get; private set; }
        [field: SerializeField] [field: Min(0)]
        public float SpawnMinDelaySec { get; private set; }
        [field: SerializeField] [field: Min(0)]
        public float SpawnDelayChangeStepSec { get; private set; }
        [field: SerializeField] [field: Min(0)]
        public float DespawnDelaySec { get; private set; }
    }
}
