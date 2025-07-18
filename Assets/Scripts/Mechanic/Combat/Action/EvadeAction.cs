using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Evade")]
public class EvadeAction : ActionBase
{
    public int evasionBoost = 30;
    public int duration = 2;

    private void OnEnable()
    {
        targetingType = TargetingType.Self;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null)
        {
            Debug.LogWarning("EvadeAction: No target provided.");
            return;
        }

        string effectName = $"EvasionUp_{evasionBoost}";
        target.statusEffectManager.AddEffect(effectName, duration);
        
        Debug.Log($"{target.characterName} is focusing! Evasion increased by {evasionBoost} for {duration} turns.");
        CombatNotificationUI.Instance.Log($"{target.characterName} is focusing! Evasion increased by {evasionBoost} for {duration} turns.");
    }
}
