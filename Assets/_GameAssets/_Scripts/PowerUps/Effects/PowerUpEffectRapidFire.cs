public class PowerUpEffectRapidFire : IPowerUpEffect
{
    private float _durationSec;
    private float _tempShootDelaySec;
    private readonly CannonController _cannonController;
    private readonly PowerUp _powerUp;

    public PowerUpEffectRapidFire(float durationSec, float tempShootDelaySec, CannonController cannonController, PowerUp powerUp)
    {
        _durationSec = durationSec;
        _tempShootDelaySec = tempShootDelaySec;
        _cannonController = cannonController;
        _powerUp = powerUp;

    }
    public void Activate()
    {
        _cannonController.EnablePowerUp(_durationSec, _tempShootDelaySec);
        _powerUp.OnActivated?.Invoke();
        _powerUp.OnActivated = null;
        _powerUp.Despawn();
    }
}
