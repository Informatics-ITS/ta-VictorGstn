using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Rootkit Smash")]
public class RootkitSmashAction : ActionBase
{
    public int damage = 50;
    public float accuracy = 0.7f; // 70% hit chance

    private void OnEnable()
    {
        targetingType = TargetingType.Enemy;
    }

    public override void PerformAction(CharacterBase user, CharacterBase target)
    {
        EnemyCharacter enemy = target as EnemyCharacter;
        if (enemy == null)
        {
            Debug.LogWarning("Rootkit Smash can only be used on enemies.");
            return;
        }

        bool alwaysHits = enemy.malwareType == MalwareType.Spyware;
        bool hit = alwaysHits || Random.value <= accuracy;

        if (!hit)
        {
            Debug.Log("Rootkit Smash missed!");
            CombatNotificationUI.Instance.Log("Rootkit Smash missed!");
            return;
        }

        
        Debug.Log($"Rootkit Smash hit {enemy.characterName} for {damage} damage.");
        CombatNotificationUI.Instance.Log($"Rootkit Smash hit {enemy.characterName} for {damage} damage.");

        if (alwaysHits)
        {
            Debug.Log("Spyware can't hide! Rootkit Smash always hits.");
            CombatNotificationUI.Instance.Log("Spyware can't hide! Rootkit Smash always hits.");
            enemy.SureHitDamage(damage, user);
        }
        else
        {
            enemy.ReceiveDamage(damage, user);
        }
    }
}
