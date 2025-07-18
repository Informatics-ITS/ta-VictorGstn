using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject howToPlayOverlay;
    private bool isPaused = false;
    public SFXPlayer sfx;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (howToPlayOverlay.activeSelf)
                howToPlayOverlay.SetActive(false); // close overlay first
            else
                TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        sfx.PlaySFX();
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        sfx.PlaySFX();
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        FadeManager.Instance.FadeToScene("Main Menu");
        sfx.PlaySFX();
    }

    public void ToggleHowToPlay()
    {
        if (howToPlayOverlay == null) return;

        bool isActive = howToPlayOverlay.activeSelf;
        howToPlayOverlay.SetActive(!isActive);
        sfx.PlaySFX();
    }

}
