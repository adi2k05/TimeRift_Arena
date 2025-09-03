using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    // The animator component
    private Animator animator;
    // The player's input component
    private PlayerInput playerInput;

    // Attack properties
    public float attackDamage = 20f;
    public float attackDuration = 0.5f;

    // A reference to the weapon's collider
    public BoxCollider weaponCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // Make sure the collider is disabled at the start
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    void Update()
    {
        // Check for attack input from the Input System
        if (playerInput.actions["Attack"].triggered)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Trigger the attack animation
        animator.SetTrigger("Attack");
        
        // Start a coroutine to handle the damage and collider
        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        // Wait for the attack animation to begin
        yield return new WaitForSeconds(0.2f);
        
        // Temporarily enable the collider to hit enemies
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
        
        // Wait for the duration of the hit
        yield return new WaitForSeconds(0.3f);
        
        // Disable the collider after the hit
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}