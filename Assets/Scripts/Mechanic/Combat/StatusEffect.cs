// StatusEffect.cs
using UnityEngine;

public class StatusEffect
{
    public string effectName;
    public int duration;

    public StatusEffect(string name, int duration)
    {
        effectName = name;
        this.duration = duration;
    }
}
