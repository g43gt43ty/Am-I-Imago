using UnityEngine;
using System.Collections;
using TMPro;

public class StoryPoint : MonoBehaviour
{
    public GameObject text;

    void OnTriggerEnter2D(Collider2D other)
    {
        text.SetActive(true);
    }
}
