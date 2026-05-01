using UnityEngine;

public class Door : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        triggered = true;

        if (GameManager.Instance != null)
            GameManager.Instance.RestartLevel();
        else
            Debug.LogError("GameManager.Instance == null");
    }
}