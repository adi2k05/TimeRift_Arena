using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public float rewindInterval = 30f;
    private float timer;

    public static bool isRewinding = false;

    private ThirdPersonController playerController;
    private PlayerInput playerInput;
    private AudioSource playerAudio; // Reference to the player's AudioSource

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<ThirdPersonController>();
            playerInput = player.GetComponent<PlayerInput>();
            
            // Get the AudioSource component
            playerAudio = player.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isRewinding) return;

        timer += Time.deltaTime;

        if (timer >= rewindInterval)
        {
            isRewinding = true;

            // Disable player controls and sound
            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
            if (playerAudio != null) playerAudio.enabled = false;

            Rewindable[] rewindables = FindObjectsOfType<Rewindable>();
            
            foreach (Rewindable rewindable in rewindables)
            {
                rewindable.StartRewind();
            }

            timer = 0f;
        }
    }
}