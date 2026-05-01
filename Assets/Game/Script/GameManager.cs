using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int restartCount = 0;
    public int RestartCount => restartCount;

    // Больше не держим прямую ссылку на GolemController
    // public GolemController golemController; // УБИРАЕМ эту строку

    private GolemController.GolemForm? pendingForm = null; // форма, которую нужно установить после загрузки

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;   // подписываемся один раз
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
        restartCount++;

        // Определяем, в какую форму нужно превратиться после перезапуска
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
                pendingForm = null; // ничего не меняем
                break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pendingForm == null)
            return;

        // Находим Голема в новой сцене
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

        pendingForm = null; // сбрасываем
    }

    public void ResetCounter()
    {
        restartCount = 0;
        pendingForm = null;
    }
}