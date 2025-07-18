using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Payload Execute")]
public class PayloadExecuteAction : ActionBase
{
    public int damage = 60;
    public int fallbackDamage = 10; // Optional: minor damage if not infected

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null) return;

        bool infected = target.statusEffectManager.HasEffect("Infected");

        if (infected)
        {
            target.ReceiveDamage(damage, user);
            CombatNotificationUI.Instance?.Log($"{user.characterName} executes a devastating payload on {target.characterName}!");
        }
        else
        {
            target.ReceiveDamage(fallbackDamage, user);
            CombatNotificationUI.Instance?.Log($"{user.characterName}'s payload misfires — target wasn't infected!");
        }
        
    }
}
