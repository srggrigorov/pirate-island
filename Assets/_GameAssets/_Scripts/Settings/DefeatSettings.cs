using UnityEngine;

namespace _GameAssets._Scripts.Settings
{
    [CreateAssetMenu(fileName = "DefeatSettings", menuName = "Settings/Defeat", order = 0)]
    public class DefeatSettings : ModuleSettings
    {
        [field: SerializeField, Min(1)]
        public int EnemiesCountForDefeat { get; private set; }
    }
}
