using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Heuristic Check")]
public class HeuristicCheckAction : ActionBase
{
    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        EnemyCharacter enemy = target as EnemyCharacter;
        if (enemy == null)
        {
            Debug.LogWarning("Heuristic Check can only be used on enemies.");
            return;
        }

        int debuffAmount = 15; // Default: reduce evasion by 20
        int duration = 2;

        if (enemy.malwareType == MalwareType.Worm)
        {
            debuffAmount = 30; // Reduce evasion to 0
            duration = 2;
            Debug.Log("Heuristic Check is VERY EFFECTIVE against Worms!");
            CombatNotificationUI.Instance.Log("Heuristic Check is VERY EFFECTIVE against Worms!");
        }
       
        enemy.statusEffectManager.AddEffect($"EvasionDown_{debuffAmount}", duration);
        Debug.Log($"Enemy evasion reduced by {debuffAmount}% for {duration} turns.");
        CombatNotificationUI.Instance.Log($"Enemy evasion reduced by {debuffAmount}% for {duration} turns.");
    }
}
