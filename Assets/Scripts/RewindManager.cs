// RewindManager.cs
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    [Header("Playback tuning")]
    [Tooltip("Desired visible rewind time (seconds). This is how long the rewind should feel like to the player.")]
    public float desiredPlaybackTime = 3f; // e.g., 3 seconds for a 10s history

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerRewind();
        }
    }

    private void TriggerRewind()
    {
        Rewindable[] all = FindObjectsOfType<Rewindable>();
        if (all == null || all.Length == 0)
        {
            Debug.LogWarning("[RewindManager] No Rewindable objects found.");
            return;
        }

        Debug.Log($"[RewindManager] Triggering rewind on {all.Length} objects. desiredPlaybackTime={desiredPlaybackTime}");

        foreach (var r in all)
        {
            if (r == null) continue;

            // Calculate frames per second recorded
            float framesPerSecondRecorded = 1f / Time.fixedDeltaTime;

            // Total frames in the recorded history
            float totalRecordedFrames = Mathf.Max(1f, r.recordTime * framesPerSecondRecorded);

            // Frames we want the rewind to take
            float desiredPlaybackFrames = Mathf.Max(1f, desiredPlaybackTime * framesPerSecondRecorded);

            // Compute speed multiplier
            float speedMultiplier = totalRecordedFrames / desiredPlaybackFrames;
            speedMultiplier = Mathf.Max(1f, speedMultiplier); // minimum 1

            r.StartRewind(speedMultiplier);
        }
    }
}