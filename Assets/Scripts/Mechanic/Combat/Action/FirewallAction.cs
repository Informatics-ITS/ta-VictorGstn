using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Firewall")]
public class FirewallAction : ActionBase
{
    private void OnEnable()
    {
        targetingType = TargetingType.Ally;
    }
    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        if (target == null)
        {
            Debug.LogWarning("FirewallAction: No target selected.");
            return;
        }
        
        target.statusEffectManager.AddEffect("Reflect", 1);
        
    }
}
