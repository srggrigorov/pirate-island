using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CannonBall : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PooledParticleSystem _hitVfxPrefab;

    private ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
    }

    private void Start()
    {
        this.OnCollisionEnterAsObservable().Subscribe(collision =>
            {
                if (collision.gameObject.TryGetComponent<IDamageable>(out var iDamageable))
                {
                    iDamageable.TakeDamage(_damage);
                }

                _objectPooler.Spawn(_hitVfxPrefab, collision.contacts[0].point, Quaternion.identity);
                _objectPooler.Despawn(this);
            }).AddTo(this);

        this.OnTriggerEnterAsObservable().Select(trigger => trigger.GetComponent<PowerUp>())
            .Where(powerUp => powerUp != null).Subscribe(powerUp =>
                {
                    powerUp.PowerUpEffect.Activate();
                    _objectPooler.Despawn(this);
                }).AddTo(this);
    }

    private void OnEnable()
    {
        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }
    }
}
