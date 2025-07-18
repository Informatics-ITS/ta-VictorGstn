using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Attack")]
public class AttackAction : ActionBase
{
    public int damage = 10;
   private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }
    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        Debug.Log(user);
        Debug.Log(target);
        Debug.Log($"{user.characterName} attacks {target.characterName} for {damage} damage.");
        CombatNotificationUI.Instance.Log($"{user.characterName} attacks {target.characterName} for {damage} damage.");
        

        target.ReceiveDamage(damage, user);
    }
}

