using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CannonBall : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _hitVfxPrefab;

    private ObjectPooler _objectPooler;

    [Inject]
    private void Construct(ObjectPooler objectPooler)
    {
        _objectPooler = objectPooler;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var iDamageable))
        {
            iDamageable.TakeDamage(_damage);
        }

        _objectPooler.Spawn(_hitVfxPrefab, collision.contacts[0].point, Quaternion.identity);
        _objectPooler.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.TryGetComponent<PowerUp>(out var powerUp))
        {
            powerUp.Activate();
            _objectPooler.Despawn(gameObject);
        }
    }
}