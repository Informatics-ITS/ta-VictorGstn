using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResultManager : MonoBehaviour
{
    public static CombatResultManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowVictory()
    {
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
    }

    public void ShowDefeat()
    {
        Time.timeScale = 0;
        defeatPanel.SetActive(true);
    }

    // UI Buttons
    public void ContinueToOverworld()
    {
        Time.timeScale = 1;
        FadeManager.Instance.FadeToScene(CombatReturnManager.returnSceneName);
    }

    public void RetryCombat()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
