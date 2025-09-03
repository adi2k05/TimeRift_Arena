using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 20f;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Hit " + other.name);
            }
        }
    }
}