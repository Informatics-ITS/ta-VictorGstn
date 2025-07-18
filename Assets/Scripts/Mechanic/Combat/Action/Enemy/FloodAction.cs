using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Actions/Flood")]
public class FloodAction : ActionBase
{
    public int baseDamagePerEnemy = 10;

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        List<CharacterBase> enemies = CombatManager.Instance.GetEnemies(user);
        enemies.RemoveAll(e => e.IsDead());

        int multiplier = enemies.Count;
        int totalDamage = baseDamagePerEnemy * multiplier;

        if (target == null)
        {
            CombatNotificationUI.Instance?.Log($"{user.characterName} tried to flood... but no target?");
            return;
        }

        target.ReceiveDamage(totalDamage, user);
        
        CombatNotificationUI.Instance?.Log($"{user.characterName} unleashed a Flood! {target.characterName} took {totalDamage} damage from {multiplier} sources.");
    }
}
