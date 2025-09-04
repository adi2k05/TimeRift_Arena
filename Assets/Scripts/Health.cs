using UnityEngine;
using System.Collections; // Required for Coroutines

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public Animator animator;
    public float deathTimer = 3f; // The time to wait before vanishing

    void Start()
    {
        currentHealth = maxHealth;
        // Get the Animator component if it's not already linked
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Trigger the GetHit animation ONLY if the character is NOT dead
            if (animator != null)
            {
                animator.SetTrigger("GetHit");
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " has died!");
        
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Start the coroutine to vanish the GameObject after a delay
        StartCoroutine(VanishAfterDeath());
    }

    IEnumerator VanishAfterDeath()
    {
        // Wait for the death animation to play
        yield return new WaitForSeconds(deathTimer);

        // Destroy the GameObject after the delay
        Destroy(gameObject);
    }
}