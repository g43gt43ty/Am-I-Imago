using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int restartCount = 0;
    public int RestartCount => restartCount;

    private GolemController.GolemForm? pendingForm = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        restartCount++;
        
        switch (restartCount)
        {
            case 1:
                Debug.Log("Первая смерть");
                pendingForm = GolemController.GolemForm.Stranger;
                break;
            case 2:
                Debug.Log("Вторая смерть");
                pendingForm = GolemController.GolemForm.Chrysalis;
                break;
            case 3:
                Debug.Log("Третья смерть");
                pendingForm = GolemController.GolemForm.Butterfly;
                break;
            default:
                Debug.Log($"Смерть №{restartCount}: без особых действий");
                pendingForm = null;
                break;
        }
        
        if (PostProcessFader.Instance != null)
        {
            PostProcessFader.Instance.FadeOut();
            yield return new WaitForSeconds(PostProcessFader.Instance.FadeDuration);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pendingForm != null)
        {
            GolemController golem = FindObjectOfType<GolemController>();
            if (golem != null)
            {
                golem.ChangeForm(pendingForm.Value);
                Debug.Log($"После загрузки применена форма: {pendingForm.Value}");
            }
            else
            {
                Debug.LogError("GolemController не найден в сцене!");
            }
            pendingForm = null;
        }
        
        if (PostProcessFader.Instance != null)
            PostProcessFader.Instance.FadeIn();
    }

    public void ResetCounter()
    {
        restartCount = 0;
        pendingForm = null;
    }
}