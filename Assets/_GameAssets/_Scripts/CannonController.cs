using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CannonController : MonoBehaviour
{

    [Space(10)] [SerializeField]
    private Camera _aimCamera;
    [SerializeField] private Transform _cannonTransform;
    [SerializeField] private Transform _shootPointTransform;
    [SerializeField] private CannonBall _ballPrefab;
    [SerializeField] private PooledParticleSystem _shootVfxPrefab;
    [SerializeField] private AudioClip _shotSfx;
    [SerializeField] private LayerMask _cameraRayCollisionMask;

    private float _shootForce;
    private float _shootDelayTimeSec;
    private bool _canShoot;
    private SoundManager _soundManager;
    private CannonSettings _settings;

    public bool CanShoot
    {
        get => _canShoot;
        set
        {
            _canShoot = value;
            if (value || _cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            else if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }

    private CancellationTokenSource _cancellationTokenSource;
    private ObjectPooler _objectPooler;
    private InputController _inputController;

    [Inject]
    private void Construct(ObjectPooler objectPooler, InputController inputController, SoundManager soundManager, AssetsManager assetsManager)
    {
        _objectPooler = objectPooler;
        _inputController = inputController;
        _soundManager = soundManager;
        _settings = assetsManager.GetModuleSettings<CannonSettings>();
        _shootDelayTimeSec = _settings.ShootDelayTimeSec;
        _shootForce = _settings.ShootForce;
    }

    private void Awake()
    {
        _aimCamera ??= Camera.main;
    }

    private void Start()
    {
        _inputController.OnShotStarted += Shoot;
    }

    private void ChangeShootDelay(float newShootDelayTimeSec)
    {
        _canShoot = false;
        _shootDelayTimeSec = newShootDelayTimeSec;
        _canShoot = true;
    }

    async private void Shoot(Vector2 pointerPosition)
    {
        if (!_canShoot)
        {
            return;
        }

        _canShoot = false;

        Ray shootRay = _aimCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(shootRay, out var raycastHit, Mathf.Infinity, _cameraRayCollisionMask))
        {
            _cannonTransform.LookAt(raycastHit.point);
        }
        else
        {
            _cannonTransform.LookAt(shootRay.origin + shootRay.direction * 100);
        }


        _objectPooler.Spawn(_ballPrefab, _shootPointTransform.position, Quaternion.identity)
            .GetComponent<Rigidbody>().AddForce(_cannonTransform.forward * _shootForce, ForceMode.Impulse);

        _objectPooler.Spawn(_shootVfxPrefab, _shootPointTransform.position, _shootPointTransform.rotation);

        _soundManager.PlaySoundOnce(_shotSfx);

        PlayerPrefs.SetInt(PlayerPrefsKeys.ShotsFired.ToString(),
            PlayerPrefs.GetInt(PlayerPrefsKeys.ShotsFired.ToString(), 0) + 1);

        try
        {
            await Task.Delay((int)(_shootDelayTimeSec * 1000), _cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        _canShoot = true;
    }

    //Made by Sergei Grigorov

    public async void EnablePowerUp(float durationSec, float tempShootDelaySec)
    {
        float startShootDelay = _shootDelayTimeSec;
        ChangeShootDelay(tempShootDelaySec);
        await Task.Delay((int)(durationSec * 1000));
        ChangeShootDelay(startShootDelay);
    }
}
