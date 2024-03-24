using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _walkDistance;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;

    private float _despawnDelay;
    private float _fadeDuration;
    private Transform _transform;
    private Vector3 _randomPoint;
    private WaitUntil _waitUntilDestinationFound;
    private WaitUntil _waitUntilDestinationReached;
    private WaitForSeconds _waitForDespawnDelay;
    private EnemySpawner _enemySpawner;
    private List<Material> _materials;

    [Inject]
    private void Construct(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
        _despawnDelay = _enemySpawner.DespawnDelay;
    }

    private void Awake()
    {
        AttachComponents();
        _transform = transform;
        _waitUntilDestinationFound = new WaitUntil(() => FindRandomDestination(out _randomPoint));
        _waitForDespawnDelay = new WaitForSeconds(_despawnDelay);
        _waitUntilDestinationReached =
            new WaitUntil(() => !_navMeshAgent.enabled || _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);
    }

    private IEnumerator SetRandomDestination()
    {
        yield return _waitUntilDestinationFound;
        if (!_navMeshAgent.enabled)
        {
            yield break;
        }
        _navMeshAgent.SetDestination(_randomPoint);
        yield return _waitUntilDestinationReached;
        StartCoroutine(SetRandomDestination());
    }

    private bool FindRandomDestination(out Vector3 randomNavMeshDestinationPoint)
    {
        Vector3 randomPointInsideSphere = Random.insideUnitSphere * _walkDistance + _transform.position;
        randomNavMeshDestinationPoint = randomPointInsideSphere;
        if (NavMesh.SamplePosition(randomPointInsideSphere, out var navMeshHit, _walkDistance, NavMesh.AllAreas))
        {
            randomNavMeshDestinationPoint = navMeshHit.position;
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        StartCoroutine(SetRandomDestination());
    }

    private void OnDisable()
    {
        SetRagdollActive(false);
    }

    private void AttachComponents()
    {
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        if (_navMeshAgent == null)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    private IEnumerator StartDespawnDelay()
    {
        yield return _waitForDespawnDelay;
        _enemySpawner.DespawnEnemy(this);
    }


    private void SetRagdollActive(bool value)
    {
        if (_ragdoll != null && _animator != null && _characterController != null && _navMeshAgent != null)
        {
            _navMeshAgent.enabled = !value;
            _characterController.enabled = !value;
            _ragdoll.SetActive(value);
            _animator.enabled = !value;
        }
    }

    #region IDamageable

    [Space(10)] [Header("IDamageable")] [SerializeField]
    private int _health;

    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            if (_health <= 0)
            {
                HandleHealthDeplete();
            }
        }
    }


    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public void HandleHealthDeplete()
    {
        StopCoroutine(SetRandomDestination());
        SetRagdollActive(true);
        StartDespawnDelay().ToObservable().Take(1).Subscribe().AddTo(this);
    }

    #endregion

}
