using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Interactable
{
    [Header("Combat Setup")]
    [SerializeField] private string combatSceneName;
    [SerializeField] private SFXPlayer sfx;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        interactionType = InteractionType.Combat;
    }

    public override void Interact()
    {
        sfx.PlaySFX();
        if (!string.IsNullOrEmpty(combatSceneName))
        {
            Debug.Log($"Loading combat scene: {combatSceneName}");
            Debug.Log("Step 1: Before setting return data");
            CombatReturnManager.lastEnemyID = gameObject.name;
            CombatReturnManager.playerReturnPosition = player.transform.position;
            CombatReturnManager.currentObjectiveIndex = QuestManager.Instance.currentObjectiveIndex;
            CombatReturnManager.returnSceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Step 2: Before fade");
            FadeManager.Instance.FadeToScene(combatSceneName);
            //SceneManager.LoadScene(combatSceneName);
        }
        else
        {
            Debug.LogWarning("Combat scene not set on enemy.");
        }
    }
}
