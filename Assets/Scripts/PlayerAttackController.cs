using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Animator animator;
    public Collider weaponCollider;   // assign weapon collider in Inspector

    private bool isAttacking = false;

    void Start()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false; // off by default
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Enable weapon collider when attack starts
        if (weaponCollider != null)
            weaponCollider.enabled = true;
    }

    // Called automatically from animation when it finishes
    // OR we can set a timer if you want fixed attack length
    public void EndAttack()
    {
        isAttacking = false;

        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }
}