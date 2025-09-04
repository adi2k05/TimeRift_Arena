using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    [Header("Animation")]
    public Animator animator;
    public float deathTimer = 3f; // Time to wait before vanishing

    [Header("Optional Health UI")]
    public Slider healthSlider; // Assign in inspector (player or enemy slider)

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update UI
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Play GetHit animation if alive
            if (animator != null)
                animator.SetTrigger("GetHit");
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(gameObject.name + " has died!");

        if (animator != null)
            animator.SetTrigger("Die");

        // Hide health slider
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);

        // Quit the game after death
        StartCoroutine(QuitAfterDeath());
    }

    IEnumerator QuitAfterDeath()
    {
        // Wait a bit for death animation
        yield return new WaitForSeconds(deathTimer);

        Debug.Log("Game over! Exiting...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#else
        Application.Quit(); // Quit build
#endif
    }
}