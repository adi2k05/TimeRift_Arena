using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    // The amount of damage this hitbox will deal
    public float attackDamage = 20f;

    void OnTriggerEnter(Collider other)
    {
        // This function is called when this hitbox touches another collider
        
        // Check if the other object is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Get the enemy's Health component
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                // Tell the enemy to take damage
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}