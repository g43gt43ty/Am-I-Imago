using UnityEngine;

public class DestroyWhenTaran : MonoBehaviour
{
    public GameObject Taran;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Taran)
        {
            if (audioSource != null && audioClip != null)
                audioSource.PlayOneShot(audioClip);
            Destroy(gameObject);
        }
    }
}