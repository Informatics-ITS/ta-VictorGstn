using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Actions/Keylogger Trap")]
public class KeyloggerTrapAction : ActionBase
{
    public int counterDamage = 40;

    private void OnEnable()
    {
        targetingType = TargetingType.Self; // doesn't require a normal target
    }

    public override void PerformAction(CharacterBase user, CharacterBase _)
    {
        List<CharacterBase> players = CombatManager.Instance.GetEnemies(user); // enemy of enemy = player
        List<CharacterBase> repeaters = new List<CharacterBase>();

        // Find players who repeated their last move
        foreach (var p in players)
        {
            PlayerCharacter pc = p as PlayerCharacter;
            if (pc != null && pc.lastAction != null && pc.lastAction == pc.previousAction)
            {
                repeaters.Add(pc);
            }
        }

        if (repeaters.Count == 0)
        {
            CombatNotificationUI.Instance?.Log($"{user.characterName} activated Keylogger Trap... but no patterns were found.");
            return;
        }

        foreach (var target in repeaters)
        {
            target.ReceiveDamage(counterDamage, user);
            CombatNotificationUI.Instance?.Log($"{user.characterName} countered {target.characterName} for repeating the same action!");
            
        }
    }
}
