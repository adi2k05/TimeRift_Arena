using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using StarterAssets;
using UnityEngine.InputSystem;

public class Rewindable : MonoBehaviour
{
    [Header("Rewind Settings")]
    public float recordTime = 10f; // seconds to record
    private int maxFrames;

    private List<Vector3> positions;
    private List<Quaternion> rotations;

    [Header("Player Settings")]
    public bool isPlayer = false; // Only player handles cameras
    public Camera thirdPersonCamera;
    public Camera topDownCamera;

    // Components
    private ThirdPersonController thirdPersonController;
    private PlayerInput playerInput;
    private NavMeshAgent navMeshAgent;
    private MonoBehaviour enemyAIScript;

    private bool isRewinding = false;

    void Start()
    {
        maxFrames = Mathf.RoundToInt(recordTime / Time.fixedDeltaTime);
        positions = new List<Vector3>(maxFrames);
        rotations = new List<Quaternion>(maxFrames);

        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<PlayerInput>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAIScript = GetComponent<MonoBehaviour>(); // for EnemyAI
    }

    void FixedUpdate()
    {
        if (isRewinding) return;

        if (positions.Count >= maxFrames)
        {
            positions.RemoveAt(0);
            rotations.RemoveAt(0);
        }

        positions.Add(transform.position);
        rotations.Add(transform.rotation);
    }

    public void StartRewind(float speedMultiplier)
    {
        if (isRewinding || positions.Count == 0) return;
        isRewinding = true;
        RewindController.isRewinding = true;

        // Disable movement/AI
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (thirdPersonController != null) thirdPersonController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (enemyAIScript != null) enemyAIScript.enabled = false;

        // Player camera switch
        if (isPlayer)
        {
            if (thirdPersonCamera != null) thirdPersonCamera.gameObject.SetActive(false);
            if (topDownCamera != null) topDownCamera.gameObject.SetActive(true);
        }

        StartCoroutine(RewindCoroutine(speedMultiplier));
    }

    private IEnumerator RewindCoroutine(float speedMultiplier)
    {
        int step = Mathf.Max(1, Mathf.RoundToInt(speedMultiplier));

        for (int i = positions.Count - 1; i >= 0; i -= step)
        {
            transform.position = positions[i];
            transform.rotation = rotations[i];
            yield return null; // smooth per-frame rewind
        }

        positions.Clear();
        rotations.Clear();
        isRewinding = false;
        RewindController.isRewinding = false;

        // Re-enable movement/AI
        if (navMeshAgent != null) navMeshAgent.enabled = true;
        if (thirdPersonController != null) thirdPersonController.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        if (enemyAIScript != null) enemyAIScript.enabled = true;

        // Switch back cameras for player
        if (isPlayer)
        {
            if (topDownCamera != null) topDownCamera.gameObject.SetActive(false);
            if (thirdPersonCamera != null) thirdPersonCamera.gameObject.SetActive(true);
        }
    }
}