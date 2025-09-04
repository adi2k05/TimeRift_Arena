using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    // The amount of damage this hitbox will deal
    public float attackDamage = 20f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log(gameObject.name + " is dealing " + attackDamage + " damage to " + other.gameObject.name);

            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}