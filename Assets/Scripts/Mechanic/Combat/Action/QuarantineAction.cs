using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Quarantine")]
public class QuarantineAction : ActionBase
{
    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        EnemyCharacter enemy = target as EnemyCharacter;
        if (enemy == null)
        {
            Debug.LogWarning("Quarantine can only be used on enemies.");
            return;
        }

        if (enemy.malwareType == MalwareType.Worm)
        {
            Debug.Log("Quarantine failed! Worm is immune to skipping turns.");
            CombatNotificationUI.Instance.Log("Quarantine failed! Worm is immune to skipping turns.");
            return;
        }

        enemy.statusEffectManager.AddEffect("Stunned", 1);
        Debug.Log($"{enemy.characterName} has been quarantined and will skip their next turn!");
        CombatNotificationUI.Instance.Log($"{enemy.characterName} has been quarantined and will skip their next turn!");

    }
}
