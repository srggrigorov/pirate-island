using UnityEngine;
using Zenject;

[DisallowMultipleComponent]
public class PowerUpRapidFire : PowerUp
{
    [SerializeField] [Min(0)]
    private float _durationSec;
    [SerializeField] [Min(0)]
    private float _tempShootDelaySec;

    [Inject]
    public void Construct(CannonController cannonController)
    {
        PowerUpEffect = new PowerUpEffectRapidFire(_durationSec, _tempShootDelaySec, cannonController, this);
    }
    public override void Despawn()
    {
        _objectPooler.Despawn(this);
    }
}
