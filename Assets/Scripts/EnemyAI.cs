using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;

    private NavMeshAgent navMeshAgent;
    private enum EnemyState { Idle, Chase, Attack }
    private EnemyState currentState;
    private bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        currentState = EnemyState.Idle;
    }

    void Update()
    {
        // -----------------------------
        // STOP EVERYTHING DURING REWIND
        // -----------------------------
        if (RewindController.isRewinding)
        {
            if (navMeshAgent != null)
                navMeshAgent.isStopped = true;

            if (animator != null)
            {
                animator.SetFloat("X_Input", 0f);
                animator.SetFloat("Y_Input", 0f);
            }

            return; // skip all AI logic while rewinding
        }

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                if (navMeshAgent != null)
                    navMeshAgent.SetDestination(player.position);

                Vector3 localVelocity = Vector3.zero;
                if (navMeshAgent != null)
                    localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);

                if (animator != null)
                {
                    animator.SetFloat("X_Input", localVelocity.x);
                    animator.SetFloat("Y_Input", localVelocity.z);
                }

                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:
                if (navMeshAgent != null)
                    navMeshAgent.isStopped = true;

                Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookAtPosition);

                if (!isAttacking)
                    StartCoroutine(AttackPlayer());

                if (distanceToPlayer > attackRange)
                {
                    if (navMeshAgent != null)
                        navMeshAgent.isStopped = false;

                    currentState = EnemyState.Chase;
                }
                break;
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);
        }

        isAttacking = false;
    }
}