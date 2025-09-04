using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // Public variables we can set in the Unity Inspector
    public Transform player;
    public Animator animator;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;

    // Internal component references
    private NavMeshAgent navMeshAgent;

    // AI States
    private enum EnemyState { Idle, Chase, Attack }
    private EnemyState currentState;
    private bool isAttacking = false;

    void Start()
    {
        // Get component references from this GameObject
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Find the player GameObject using its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Set the initial state
        currentState = EnemyState.Idle;
    }

    void Update()
    {
        // If there's no player, don't do anything
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // State Machine logic
        switch (currentState)
        {
            case EnemyState.Idle:
                // Check if the player is within chasing range
                if (distanceToPlayer <= chaseRange)
                {
                    currentState = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                // Set the destination to the player's position
                navMeshAgent.SetDestination(player.position);

                // Convert world velocity to local space for the blend tree
                Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
                animator.SetFloat("X_Input", localVelocity.x);
                animator.SetFloat("Y_Input", localVelocity.z);

                // If the player is within attacking range, change to Attack state
                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:
                // Stop the agent from moving
                navMeshAgent.isStopped = true;

                // Look at the player before attacking
                Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookAtPosition);

                // If not already attacking, start the attack routine
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }

                // If the player moves out of attack range, go back to chasing
                if (distanceToPlayer > attackRange)
                {
                    navMeshAgent.isStopped = false;
                    currentState = EnemyState.Chase;
                }
                break;
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        // Set the trigger to play the single attack animation
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.0f);

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log(gameObject.name + " is attacking and attempting to deal " + attackDamage + " damage.");

            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        isAttacking = false;
    }
}