using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Infect")]
public class InfectAction : ActionBase
{
    public int damage = 15;

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null) return;

        // Deal damage
        target.ReceiveDamage(damage, user);

        // Apply "Infected" status for 3 turns
        target.statusEffectManager.AddEffect("Infected", 3);
        
        CombatNotificationUI.Instance?.Log($"{user.characterName} infected {target.characterName}!");
    }
}
