using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Actions/Mimic")]
public class MimicAction : ActionBase
{
    [SerializeField] private Sprite[] mimicSprites;
    [SerializeField] private string[] mimicNames;
    [SerializeField] private int confusionDuration = 2;

    private void OnEnable()
    {
        targetingType = TargetingType.Self;
    }

    public override void PerformAction(CharacterBase user, CharacterBase _)
    {
        EnemyCharacter enemy = user as EnemyCharacter;
        if (enemy == null || mimicSprites.Length == 0 || mimicNames.Length == 0) return;

        int index = Random.Range(0, Mathf.Min(mimicSprites.Length, mimicNames.Length));
        enemy.ApplyMimic(mimicSprites[index], mimicNames[index]);

        enemy.statusEffectManager.AddEffect("Confusion", confusionDuration);
        
        CombatNotificationUI.Instance?.Log($"{enemy.characterName} mimicked a party member!");
    }
}
