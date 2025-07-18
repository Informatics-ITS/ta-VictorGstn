using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Actions/Replicate")]
public class ReplicateAction : ActionBase
{
    public int healthCost = 15;

    private void OnEnable()
    {
        targetingType = TargetingType.Self;
    }

    public override void PerformAction(CharacterBase user, CharacterBase _)
    {
        EnemyCharacter enemy = user as EnemyCharacter;
        if (enemy == null || enemy.IsDead()) return;

        // Find available spawn points
        List<Transform> availableSpots = CombatManager.Instance.GetAvailableEnemySpawns();
        if (availableSpots.Count == 0)
        {
            CombatNotificationUI.Instance?.Log($"{enemy.characterName} tried to replicate... but there’s no space!");
            return;
        }

        // Drain HP
        user.ReceiveDamage(healthCost, user);
        

        // Instantiate clone
        GameObject cloneObj = GameObject.Instantiate(enemy.gameObject, availableSpots[0].position, Quaternion.identity);
        EnemyCharacter clone = cloneObj.GetComponent<EnemyCharacter>();

        if (clone != null)
        {
            clone.characterName = enemy.characterName + " Clone";

            CombatManager.Instance.RegisterEnemy(clone);
            CombatNotificationUI.Instance?.Log($"{enemy.characterName} replicated itself!");
            CombatSFXLibrary.Instance.PlaySummon();

        }
    }
}
