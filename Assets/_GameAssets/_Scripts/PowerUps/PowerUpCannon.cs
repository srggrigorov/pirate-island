using UnityEngine;

[DisallowMultipleComponent]
public class PowerUpCannon : PowerUp
{
    [SerializeField] [Min(0)] private float _durationSec;
    [SerializeField] [Min(0)] private float _tempShootDelaySec;
    private CanonController _canonController;

    public override void Activate()
    {
        _canonController.EnablePowerUp(_durationSec, _tempShootDelaySec);
        OnActivated?.Invoke();
        OnActivated = null;
        _objectPooler.Despawn(gameObject);
    }

    private void OnEnable()
    {
        _canonController ??= FindObjectOfType<CanonController>();
    }
}