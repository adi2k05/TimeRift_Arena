using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform playerTransform;

    public float attackRange = 2f;
    public float attackDamage = 10f;
    private bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                navMeshAgent.isStopped = true;
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(playerTransform.position);
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        
        yield return new WaitForSeconds(1.0f);

        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            Health playerHealth = playerTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        isAttacking = false;
    }
}