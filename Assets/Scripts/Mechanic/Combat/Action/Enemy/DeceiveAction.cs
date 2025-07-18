using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Deceive")]
public class DeceiveAction : ActionBase
{
    public int debuffAmount = 20; // Reduce defense by 20
    public int duration = 3;

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null) return;

        string effectName = $"DefenseDown_{debuffAmount}";
        target.statusEffectManager.AddEffect(effectName, duration);
        
        CombatNotificationUI.Instance?.Log($"{user.characterName} deceives {target.characterName}, lowering their defense!");
    }
}
