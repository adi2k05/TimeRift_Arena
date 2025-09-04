using UnityEngine;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine.InputSystem;

public class Rewindable : MonoBehaviour
{
    private List<Vector3> positions;
    private List<Quaternion> rotations;
    private bool isRewinding = false;

    // References to the two cameras
    public Camera thirdPersonCamera;
    public Camera topDownCamera;

    // Component references on this GameObject
    private ThirdPersonController thirdPersonController;
    private PlayerInput playerInput;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        // Get references to the components on this GameObject
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<PlayerInput>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        // Only record if we are not currently rewinding
        if (!isRewinding)
        {
            positions.Add(transform.position);
            rotations.Add(transform.rotation);
        }
    }
    
    public void StartRewind()
    {
        // Prevent starting a new rewind if one is already in progress
        if (isRewinding) return;

        isRewinding = true;
        
        // Disable conflicting components during the rewind
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (thirdPersonController != null) thirdPersonController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        
        // Disable the third-person camera and enable the top-down camera
        if (thirdPersonCamera != null) thirdPersonCamera.enabled = false;
        if (topDownCamera != null) topDownCamera.enabled = true;

        StartCoroutine(Rewind());
    }

    private System.Collections.IEnumerator Rewind()
    {
        try
        {
            // Rewind through the stored data
            for (int i = positions.Count - 1; i >= 0; i -= 4)
            {
                transform.position = positions[i];
                transform.rotation = rotations[i];
                
                // Wait for a fraction of a second to make the rewind visible
                yield return new WaitForSeconds(0.01f);
            }
        }
        finally
        {
            positions.Clear();
            rotations.Clear();
            isRewinding = false;

            // Re-enable components after the rewind is done
            if (navMeshAgent != null) navMeshAgent.enabled = true;
            if (thirdPersonController != null) thirdPersonController.enabled = true;
            if (playerInput != null) playerInput.enabled = true;
            
            // Disable the top-down camera and re-enable the third-person camera
            if (topDownCamera != null) topDownCamera.enabled = false;
            if (thirdPersonCamera != null) thirdPersonCamera.enabled = true;
        }
    }
}