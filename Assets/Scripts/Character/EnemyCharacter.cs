using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyCharacter : CharacterBase
{
    public MalwareType malwareType;
    [Header("Enemy Actions")]
    public List<EnemyActionEntry> actions = new List<EnemyActionEntry>();
    private Sprite originalSprite;
    private string originalName;
    private bool isMimicked = false;
    private bool started = false; 

    public override IEnumerator TakeTurn()
    {
        if (!started)
        {
            originalSprite = GetComponentInChildren<SpriteRenderer>().sprite;
            originalName = characterName;
        }
        bool wasStunned = statusEffectManager.HasEffect("Stunned");
        // Base logic (e.g., status check)
            yield return base.TakeTurn();
        if (IsDead()|| wasStunned) yield break;

        TurnJumpController jumper = GetComponent<TurnJumpController>();
        if (jumper != null)
#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
            yield return jumper.JumpIn(this is PlayerCharacter);
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type
        yield return new WaitForSeconds(1.5f);
        // Pick an action
        ActionBase selectedAction = PickWeightedAction();
        if (selectedAction == null)
        {
            Debug.LogWarning($"{characterName} has no valid actions!");
            yield break;
        }

        // Pick a target
        TargetingType targetType = selectedAction.targetingType;
        List<CharacterBase> candidates = CombatManager.Instance.GetTargets(this, targetType);
        if (candidates.Count == 0)
        {
            Debug.LogWarning($"{characterName} has no valid targets for {selectedAction.name}");
            yield break;
        }

        CharacterBase target = candidates[Random.Range(0, candidates.Count)];

        // Perform the action
        Debug.Log($"{characterName} uses {selectedAction.actionName} on {target.characterName}");
        CombatNotificationUI.Instance.Log($"{characterName} uses {selectedAction.actionName} on {target.characterName}");
        selectedAction.PerformAction(this, target);
        
        if (jumper != null)
            yield return jumper.JumpBack();
        yield return new WaitForSeconds(1.5f);
    }

    private ActionBase PickWeightedAction()
    {
        int totalWeight = 0;
        foreach (var entry in actions)
            totalWeight += entry.weight;

        if (totalWeight == 0) return null;

        int roll = Random.Range(1, totalWeight + 1);
        int cumulative = 0;

        foreach (var entry in actions)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.action;
        }

        return null;
    }

    public void ApplyMimic(Sprite mimicSprite, string mimicName)
    {   
        
        GetComponentInChildren<SpriteRenderer>().sprite = mimicSprite;
        GetComponentInChildren<SpriteRenderer>().flipX = true;
        characterName = mimicName;
        isMimicked = true;
    }

    public void RevertMimic()
    {
        if (!isMimicked) return;
        GetComponentInChildren<SpriteRenderer>().sprite = originalSprite;
        GetComponentInChildren<SpriteRenderer>().flipX = false;
        characterName = originalName;
        isMimicked = false;
    }


}
