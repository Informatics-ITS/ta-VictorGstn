using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Actions/Network Propagation")]
public class NetworkPropagationAction : ActionBase
{
    public int buffAmount = 20;
    public int duration = 3;

    private void OnEnable()
    {
        targetingType = TargetingType.Self; // but selects another ally inside the action
    }

    public override void PerformAction(CharacterBase user, CharacterBase _)
    {
        // Get other enemies (excluding self)
        List<CharacterBase> allies = CombatManager.Instance.GetAllies(user);
        allies.Remove(user);

        if (allies.Count == 0)
        {
            CombatNotificationUI.Instance?.Log($"{user.characterName} tried to propagate, but no allies were found.");
            allies.Add(user);
            return;
        }

        // Choose a random ally
        CharacterBase chosen = allies[Random.Range(0, allies.Count)];

        // Apply buff
        string effectName = $"AttackUp_{buffAmount}";
        chosen.statusEffectManager.AddEffect(effectName, duration);

        CombatNotificationUI.Instance?.Log($"{user.characterName} spread strength to {chosen.characterName}!");
        allies.Add(user);
    }
}
