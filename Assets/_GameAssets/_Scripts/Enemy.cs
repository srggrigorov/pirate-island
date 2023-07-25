using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private IEnumerator SetRandomDestination()
    {
        Vector3 randomPoint = Vector3.zero;
        yield return new WaitUntil(() => EnemySpawner.Instance.GetRandomPointOnNavMesh(out randomPoint));

        _navMeshAgent.SetDestination(randomPoint);

        yield return new WaitUntil(() => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);
        StartCoroutine(SetRandomDestination());
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
        EnemySpawner.Instance.DespawnEnemy(this);
    }

    #endregion
}