using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance;

    public GameObject actionPanel;
    public GameObject actionButtonPrefab;

    private List<Button> actionButtons = new List<Button>();
    private int currentSelection = 0;
    private PlayerCharacter currentPlayer;
    private bool inputActive = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowActions(List<ActionBase> actions, PlayerCharacter player)
    {
        currentPlayer = player;
        currentSelection = 0;
        inputActive = true;
        actionPanel.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in actionPanel.transform)
        {
            Destroy(child.gameObject);
        }

        actionButtons.Clear();

        // Create up to 4 action buttons
        for (int i = 0; i < actions.Count && i < 4; i++)
        {
            GameObject btnObj = Instantiate(actionButtonPrefab, actionPanel.transform);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = actions[i].actionName;

            int index = i; // capture index for closure
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectAction(index);
            });

            actionButtons.Add(btnObj.GetComponent<Button>());
        }

        HighlightCurrent();
    }

    private void Update()
    {
        if (!inputActive || actionButtons.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelection(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveSelection(1);
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveSelection(-2);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveSelection(2);
        if (Input.GetKeyDown(KeyCode.Z)) SelectAction(currentSelection);
    }

    void MoveSelection(int offset)
    {
        int newIndex = currentSelection + offset;
        if (newIndex < 0 || newIndex >= actionButtons.Count) return;

        currentSelection = newIndex;
        HighlightCurrent();
    }

    void HighlightCurrent()
    {
        for (int i = 0; i < actionButtons.Count; i++)
        {
            var colors = actionButtons[i].colors;
            colors.normalColor = (i == currentSelection) ? Color.yellow : Color.white;
            actionButtons[i].colors = colors;
        }
    }

    void SelectAction(int index)
    {
        if (index < 0 || index >= actionButtons.Count) return;

        inputActive = false;
        actionPanel.SetActive(false);

        currentPlayer.SelectAction(currentPlayer.actions[index]);
    }
}
