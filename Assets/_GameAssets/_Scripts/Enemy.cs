using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _walkDistance;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Transform _transform;
    private Vector3 _randomPoint;
    private WaitUntil _waitUntilDestinationFound;
    private WaitUntil _waitUntilDestinationReached;
    private EnemySpawner _enemySpawner;

    [Inject]
    private void Construct(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
    }

    private void Awake()
    {
        _transform = transform;
        _waitUntilDestinationFound = new WaitUntil(() => FindRandomDestination(out _randomPoint));
        _waitUntilDestinationReached =
            new WaitUntil(() => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);
    }

    private IEnumerator SetRandomDestination()
    {
        yield return _waitUntilDestinationFound;
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
        StopCoroutine(SetRandomDestination());
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
        _enemySpawner.DespawnEnemy(this);
    }

    #endregion
}