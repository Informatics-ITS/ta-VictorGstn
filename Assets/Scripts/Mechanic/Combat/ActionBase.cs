using UnityEngine;

public abstract class ActionBase : ScriptableObject
{
    public string actionName;
    public TargetingType targetingType = TargetingType.Enemy;


    public abstract void PerformAction(CharacterBase user, CharacterBase target);
    
}

