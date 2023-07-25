using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CanonBall : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _hitVfxPrefab;

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

        ObjectPooler.Instance.Spawn(_hitVfxPrefab, collision.contacts[0].point, Quaternion.identity);
        ObjectPooler.Instance.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.TryGetComponent<PowerUp>(out var powerUp))
        {
            powerUp.Activate();
            ObjectPooler.Instance.Despawn(gameObject);
        }
    }
}