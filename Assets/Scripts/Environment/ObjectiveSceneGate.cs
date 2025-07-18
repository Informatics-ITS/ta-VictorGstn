using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveSceneGate : MonoBehaviour
{
    [Header("Objective Requirement")]
    [SerializeField] private int requiredObjectiveIndex = 0;

    [Header("Scene Info")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private Vector3 playerSpawnPosition = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {

        int currentObjective = QuestManager.Instance.currentObjectiveIndex;

        if (currentObjective >= requiredObjectiveIndex)
        {
        
            // Transition
            FadeManager.Instance.FadeToScene(targetSceneName);
        }

    }
}
