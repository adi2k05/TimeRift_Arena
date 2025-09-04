using UnityEngine;
using System.Collections;

public class PlayerAttackController : MonoBehaviour
{
    // A reference to the Animator component on your player
    public Animator animator;
    
    // A reference to the attack hitbox's script
    public MeleeHitbox hitboxScript;

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Set the "Attack" trigger in the Animator to play the animation
            animator.SetTrigger("Attack");
            
            // Start the coroutine to handle the attack timing
            StartCoroutine(PerformAttack());
        }
    }
    
    IEnumerator PerformAttack()
    {
        // Disable the hitbox at the start of the attack to prevent multiple hits
        if (hitboxScript != null)
        {
            hitboxScript.enabled = false;
        }

        // Wait for the exact moment the attack hits
        yield return new WaitForSeconds(0.44f);
        
        // Temporarily enable the hitbox to check for collisions
        if (hitboxScript != null)
        {
            hitboxScript.enabled = true;
        }

        // Keep the hitbox enabled for a very short moment
        yield return new WaitForSeconds(0.1f);

        // Disable the hitbox after the attack has landed
        if (hitboxScript != null)
        {
            hitboxScript.enabled = false;
        }
    }
}