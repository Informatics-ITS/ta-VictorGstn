using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI")]
    public GameObject objectiveUI;
    public TextMeshProUGUI objectiveText;

    [Header("Objective Data")]
    public ObjectiveScript currentObjectiveScript;

    public int currentObjectiveIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (objectiveUI != null)
            objectiveUI.SetActive(false);
        
       
    }

    void Start()
    {
        if (currentObjectiveScript != null)
            StartObjectives(currentObjectiveScript);
    }

    public void StartObjectives(ObjectiveScript script)
    {
        currentObjectiveScript = script;
        
        if (objectiveUI != null)
            objectiveUI.SetActive(true);

    
        UpdateObjectiveText();
    }

    public void AdvanceObjective()
    {
        currentObjectiveIndex++;
        Debug.Log("Advance Obj");
        if (currentObjectiveIndex >= currentObjectiveScript.objectives.Length)
        {
            objectiveText.text = "Objective Complete!";
            return;
        }

        UpdateObjectiveText();
    }

    private void UpdateObjectiveText()
    {
        if (currentObjectiveScript == null || currentObjectiveIndex >= currentObjectiveScript.objectives.Length)
            return;

        objectiveText.text = currentObjectiveScript.objectives[currentObjectiveIndex];
    }

    public void SetObjectiveIndex(int index)
    {
        currentObjectiveIndex = index;
        UpdateObjectiveText();
    }
}
