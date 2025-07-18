using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Actions/Zombie Uprising")]
public class ZombieUprisingAction : ActionBase
{
    public int maxZombies = 2;

    private void OnEnable()
    {
        targetingType = TargetingType.Self; // Spawns allies, doesn't target
    }

    public override void PerformAction(CharacterBase user, CharacterBase _)
    {
        List<Transform> availableSpots = CombatManager.Instance.GetAvailableEnemySpawns();
        int spawnCount = Mathf.Min(availableSpots.Count, maxZombies);

        if (spawnCount == 0)
        {
            CombatNotificationUI.Instance?.Log($"{user.characterName} tried to summon, but there's no space!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject zombieGO = GameObject.Instantiate(CombatManager.Instance.ZombiePrefab, availableSpots[i].position, Quaternion.identity);
            EnemyCharacter zombie = zombieGO.GetComponent<EnemyCharacter>();

            if (zombie != null)
            {
                CombatManager.Instance.RegisterEnemy(zombie);
                CombatNotificationUI.Instance?.Log($"{user.characterName} summoned {zombie.characterName}!");
                CombatSFXLibrary.Instance.PlaySummon();

            }
        }
        
    }
}
