using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CanonController : MonoBehaviour
{
    [SerializeField] private float _shootForce;
    [SerializeField] private float _shootDelayTimeSec;

    [Space(10)] [SerializeField] private Camera _aimCamera;
    [SerializeField] private Transform _canonTrasfrom;
    [SerializeField] private Transform _shootPointTransform;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _shootVfxPrefab;
    [SerializeField] private AudioClip _shotSfx;
    [SerializeField] private LayerMask _cameraRayCollisionMask;

    private bool _canShoot;

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

    private void Awake()
    {
        _aimCamera ??= Camera.main;
    }

    private void Start()
    {
        InputController.Instance.OnShotStarted += Shoot;
    }

    public void ChangeShootDelay(float newShootDelayTimeSec)
    {
        _canShoot = false;
        _shootDelayTimeSec = newShootDelayTimeSec;
        _canShoot = true;
    }

    private async void Shoot(Vector2 pointerPosition)
    {
        if (!_canShoot)
        {
            return;
        }

        _canShoot = false;

        Ray shootRay = _aimCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(shootRay, out var raycastHit, Mathf.Infinity, _cameraRayCollisionMask))
        {
            _canonTrasfrom.LookAt(raycastHit.point);
        }
        else
        {
            _canonTrasfrom.LookAt(shootRay.origin + shootRay.direction * 100);
        }


        ObjectPooler.Instance.Spawn(_ballPrefab, _shootPointTransform.position, Quaternion.identity)
            .GetComponent<Rigidbody>().AddForce(_canonTrasfrom.forward * _shootForce, ForceMode.Impulse);

        ObjectPooler.Instance.Spawn(_shootVfxPrefab, _shootPointTransform.position, _shootPointTransform.rotation);

        SoundManager.Instance.PlaySoundOnce(_shotSfx);
        
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