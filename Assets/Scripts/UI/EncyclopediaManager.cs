using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EncyclopediaManager : MonoBehaviour
{
    [SerializeField] private GameObject encyclopediaUi;
    private bool isPaused = false;
    public TMP_Text infoText;
    public SFXPlayer sfx;
    public void ShowInfo(InfoEntry entry)
    {
        infoPanel.SetActive(true);
        infoText.text = entry.infoText;
    }

    public GameObject malwareSubmenu;
    public GameObject playerSubmenu;
    public GameObject infoPanel;
    public bool AdvanceObjective = false;

    public void ShowMalwareSubmenu()
    {
        malwareSubmenu.SetActive(true);
        playerSubmenu.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void ShowPlayerSubmenu()
    {
        malwareSubmenu.SetActive(false);
        playerSubmenu.SetActive(true);
        infoPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (encyclopediaUi.activeSelf)
                Resume();
            else
                ToggleEncyclopedia();
        }
    }

    public void ToggleEncyclopedia()
    {
        isPaused = !isPaused;
        encyclopediaUi.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        sfx.PlaySFX();
        if (AdvanceObjective)
            QuestManager.Instance.AdvanceObjective();
    }

    public void Resume()
    {
        isPaused = false;
        encyclopediaUi.SetActive(false);
        Time.timeScale = 1;
        sfx.PlaySFX();
    }


}
