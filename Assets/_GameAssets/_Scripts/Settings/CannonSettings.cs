
using UnityEngine;
namespace _GameAssets._Scripts.Settings
{
    [CreateAssetMenu(fileName = "CannonSettings", menuName = "Settings/Cannon")]
    public class CannonSettings : ModuleSettings
    {
        [field: SerializeField]
        public float ShootForce { get; private set; }
        [field: SerializeField]
        public float ShootDelayTimeSec { get; private set; }
    }
}
