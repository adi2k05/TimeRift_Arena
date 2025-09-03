using UnityEngine;
using System.Collections.Generic;
using StarterAssets; // Needed for the ThirdPersonController
using UnityEngine.InputSystem; // Needed for the PlayerInput

public class Rewindable : MonoBehaviour
{
    private List<Vector3> positions;
    private List<Quaternion> rotations;

    void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
    }

    void Update()
    {
        if (!TimeManager.isRewinding)
        {
            positions.Add(transform.position);
            rotations.Add(transform.rotation);
        }
    }
    
    public void StartRewind()
    {
        StartCoroutine(Rewind());
    }

    System.Collections.IEnumerator Rewind()
    {
        // Playback the stored data in reverse, skipping 3 frames each step
        // to make the rewind 4 times faster
        for (int i = positions.Count - 1; i >= 0; i -= 4)
        {
            transform.position = positions[i];
            transform.rotation = rotations[i];
            
            // Wait for a fraction of a second to make the rewind visible
            yield return new WaitForSeconds(0.01f);
        }

        positions.Clear();
        rotations.Clear();

        // The rewind is done, so we set the variable back to false
        TimeManager.isRewinding = false;

        // Check if this object is the player and re-enable controls
        if (gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<ThirdPersonController>().enabled = true;
                player.GetComponent<PlayerInput>().enabled = true;
            }
        }
    }
}