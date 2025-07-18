using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Payload Purge")]
public class PayloadPurgeAction : ActionBase
{
    public int damage = 30;

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        EnemyCharacter enemy = target as EnemyCharacter;
        if (enemy == null)
        {
            Debug.LogWarning("Payload Purge can only be used on enemies.");
            return;
        }

        
        //  Remove buffs 
        enemy.statusEffectManager.RemoveBuffs();
        Debug.Log("Enemy buffs purged.");
        CombatNotificationUI.Instance.Log("Enemy buffs purged.");

        //  Trojan-specific message
        if (enemy.malwareType == MalwareType.Trojan)
        {
            enemy.SureHitDamage(damage, user);
            Debug.Log("Trojan disguise bypassed! Payload hits cleanly.");
            CombatNotificationUI.Instance.Log("Trojan disguise bypassed! Payload hits cleanly.");
        }
        else
        {
            enemy.ReceiveDamage(damage, user);
            Debug.Log($"Payload Purge dealt {damage} damage to {enemy.characterName}.");
            CombatNotificationUI.Instance.Log($"Payload Purge dealt {damage} damage to {enemy.characterName}.");
        }
        
    }
}
