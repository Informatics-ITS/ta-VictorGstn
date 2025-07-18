using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyCleanupOnReturn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private QuestManager objectiveManager;
    private int ObjectiveIndex;
    void Start()
    {
        if (CombatReturnManager.returnSceneName == SceneManager.GetActiveScene().name)
        {
            // Restore player position
            player.transform.position = CombatReturnManager.playerReturnPosition;

            // Restore objective index
            ObjectiveIndex = CombatReturnManager.currentObjectiveIndex;
            objectiveManager.SetObjectiveIndex(ObjectiveIndex);
            objectiveManager.AdvanceObjective();

            // Remove defeated enemy
            GameObject defeated = GameObject.Find(CombatReturnManager.lastEnemyID);
            if (defeated != null)
                Destroy(defeated);

            // Clear return data 
            CombatReturnManager.returnSceneName = null;
            CombatReturnManager.lastEnemyID = null;
        }
    }
}
