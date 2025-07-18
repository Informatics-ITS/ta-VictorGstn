using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Signature Scan")]
public class SignatureScanAction : ActionBase
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
            Debug.LogWarning("Signature Scan can only be used on enemies.");
            return;
        }
        CombatNotificationUI.Instance.Log($"Signature Scan reveals target type: {enemy.malwareType}");
        Debug.Log($"Signature Scan reveals target type: {enemy.malwareType}");

        if (enemy.malwareType == MalwareType.Botnet)
        {
            Debug.Log("Signature Scan is not effective against Botnet. No effect.");
            CombatNotificationUI.Instance.Log("Signature Scan is not effective against Botnet. No effect.");
            return;
        }

        int duration = (enemy.malwareType == MalwareType.Virus) ? 4 : 2;
        enemy.statusEffectManager.AddEffect("Weaken", duration);

        Debug.Log($"Enemy weakened for {duration} turns.");
        CombatNotificationUI.Instance.Log($"Enemy weakened for {duration} turns.");
    }
}
