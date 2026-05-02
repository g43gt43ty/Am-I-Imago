using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PostProcessFader : MonoBehaviour
{
    public static PostProcessFader Instance { get; private set; }

    [Header("Настройки")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Volume globalVolume;

    private ColorAdjustments colorAdjustments;
    
    public float FadeDuration => fadeDuration;

    private void Awake()
    {
        // Синглтон
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (globalVolume == null)
            globalVolume = GetComponent<Volume>();

        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = true;
            colorAdjustments.postExposure.value = -10f;
        }
        else
        {
            Debug.LogError("Не удалось найти Color Adjustments в профиле Volume!");
        }
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, -10f));
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(-10f, 0f));
    }

    private IEnumerator FadeRoutine(float fromEV, float toEV)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float curvedT = fadeCurve.Evaluate(t);
            colorAdjustments.postExposure.value = Mathf.Lerp(fromEV, toEV, curvedT);
            yield return null;
        }
        colorAdjustments.postExposure.value = toEV;
    }
}