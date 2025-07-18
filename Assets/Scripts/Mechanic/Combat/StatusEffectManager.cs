using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();
    [SerializeField] private StatusEffectDisplay display;
    public void AddEffect(string name, int duration)
    {
        // If effect exists, refresh it
        StatusEffect existing = activeEffects.Find(e => e.effectName == name);
        if (existing != null)
        {
            existing.duration = duration;
        }
        else
        {
            activeEffects.Add(new StatusEffect(name, duration));
            if (IsBuff(name))
                CombatSFXLibrary.Instance.PlayBuff();
            else
                CombatSFXLibrary.Instance.PlayDebuff();

            UpdateStatusDisplay();
        }

        Debug.Log($"{name} effect applied for {duration} turns.");
        UpdateStatusDisplay();
    }

    public bool HasEffect(string name)
    {
        return activeEffects.Exists(e => e.effectName == name);
    }
    public void RemoveEffect(string effectName)
    {
        activeEffects.RemoveAll(e => e.effectName == effectName);
        Debug.Log($"Effect {effectName} removed.");
        UpdateStatusDisplay();
    }

    public void TickDown()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].duration--;
            if (activeEffects[i].duration <= 0)
            {
                Debug.Log($"Effect {activeEffects[i].effectName} expired.");
                // After removing the "Confusion" effect
                if (activeEffects[i].effectName == "Confusion")
                {
                    if (GetComponent<EnemyCharacter>())
                        GetComponent<EnemyCharacter>().RevertMimic();
                }
                activeEffects.RemoveAt(i);
                UpdateStatusDisplay();
            }
        }
    }

    public void RemoveBuffs()
    {
        activeEffects.RemoveAll(effect =>
            effect.effectName == "Dodge" ||
            effect.effectName == "Reflect"
        // Add more buff names as needed
        );

        Debug.Log("All buffs removed.");
        UpdateStatusDisplay();
    }


    public List<StatusEffect> GetAllEffects()
    {
        return activeEffects;
    }

    private void UpdateStatusDisplay()
    {
        
        if (display != null)
        {
            List<string> current = new();
            foreach (var effect in GetAllEffects())
            {
                Debug.Log($"current effect {effect.effectName}");
                current.Add(effect.effectName); // Or add formatted string like "Poison (2)"
            }
            Debug.Log(current);
            display.ShowStatusEffects(current);
        }
    }
    private bool IsBuff(string effect)
    {
        return effect.Contains("Up") || effect == "Confusion" || effect == "Reflect";
    }


}
