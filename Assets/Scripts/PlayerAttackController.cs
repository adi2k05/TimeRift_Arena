using UnityEngine;

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
        }
    }
    
    // This function is called by the animation event at the start of the attack
    public void StartAttack()
    {
        if (hitboxScript != null)
        {
            hitboxScript.enabled = true;
        }
    }

    // This function is called by the animation event at the end of the attack
    public void EndAttack()
    {
        if (hitboxScript != null)
        {
            hitboxScript.enabled = false;
        }
    }
}