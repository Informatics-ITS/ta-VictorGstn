using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Steal")]
public class StealAction : ActionBase
{
    public int damage = 20;
    public int evasionBoost = 25;
    public int evasionDuration = 2;

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null) return;

        // Deal damage
        target.ReceiveDamage(damage, user);

        // Buff self evasion
        string effectName = $"EvasionUp_{evasionBoost}";
        user.statusEffectManager.AddEffect(effectName, evasionDuration);

        CombatNotificationUI.Instance?.Log($"{user.characterName} strikes and vanishes, raising their evasion!");
        
    }
}
