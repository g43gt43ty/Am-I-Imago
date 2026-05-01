using UnityEngine;

public class DestroyWhenTaran : MonoBehaviour
{
    public GameObject Taran;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Taran)
            Destroy(gameObject);
    }
}