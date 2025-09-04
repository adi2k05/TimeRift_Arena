using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MeleeHitbox collided with: " + other.name);

        // Only act on enemies
        if (other.CompareTag("Enemy"))
        {
            // Look for Health component in the parent object (covers all colliders)
            Health enemyHealth = other.GetComponentInParent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("✅ Enemy took damage: " + other.name + " for " + damage);
            }
            else
            {
                Debug.LogWarning("⚠️ Enemy object has no Health component in parent!");
            }
        }
    }
}