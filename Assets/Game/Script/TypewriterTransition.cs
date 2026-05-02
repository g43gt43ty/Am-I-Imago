using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TypewriterTransition : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private List<TextEntry> textEntries = new List<TextEntry>();
    [SerializeField] private float letterDelay = 0.05f;

    [Header("Звук")]
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Затухание")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Переход")]
    [SerializeField] private string nextSceneName = "Level1";

    private bool textFinished = false;
    private bool waitingForInput = false;
    private int currentTextIndex = 0;

    private void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (canvasGroup == null)
            canvasGroup = GetComponentInParent<CanvasGroup>();

        if (textEntries.Count > 0)
            StartCoroutine(TypeText(textEntries[0].text));
    }

    private IEnumerator TypeText(string fullText)
    {
        displayText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            displayText.text += fullText[i];
            PlayTypeSound();
            yield return new WaitForSeconds(letterDelay);
        }

        textFinished = true;
        waitingForInput = true;
    }

    private void PlayTypeSound()
    {
        if (typeSound != null && audioSource != null)
            audioSource.PlayOneShot(typeSound);
    }

    private void Update()
    {
        if (waitingForInput && textFinished && Input.anyKeyDown)
        {
            waitingForInput = false;
            textFinished = false;
            
            currentTextIndex++;

            if (currentTextIndex < textEntries.Count)
            {
                displayText.text = "";
                StartCoroutine(TypeText(textEntries[currentTextIndex].text));
            }
            else
            {
                StartCoroutine(FadeAndLoad());
            }
        }
    }

    private IEnumerator FadeAndLoad()
    {
        if (canvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                yield return null;
            }
        }
        else
        {
            Color textColor = displayText.color;
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                displayText.color = textColor;
                yield return null;
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}

// класс для удобного редактирования текста в инспекторе
[System.Serializable]
public class TextEntry
{
    [TextArea(3, 10)]
    public string text;
}