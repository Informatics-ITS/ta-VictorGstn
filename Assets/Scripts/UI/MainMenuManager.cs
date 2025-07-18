using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayOverlay;

    public void StartGame()
    {
        //SceneManager.LoadScene("Overworld"); // Replace with your actual start scene
        FadeManager.Instance.FadeToScene("Base_Intro");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToggleHowToPlay()
    {
        if (howToPlayOverlay == null) return;

        bool isActive = howToPlayOverlay.activeSelf;
        howToPlayOverlay.SetActive(!isActive);
    }

}
