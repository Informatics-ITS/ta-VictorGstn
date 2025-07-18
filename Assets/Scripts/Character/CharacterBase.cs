using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public abstract class CharacterBase : MonoBehaviour
{
    public string characterName;
    public int maxHP = 50;
    public int currentHP = 50;
    public Slider healthSlider;

    public int accuracy = 100; // Base hit chance
    public int evasion = 20;    // Chance to avoid being hit

    [SerializeField] private GameObject turnHighlight;

    public bool hasDied = false;
    public void UpdateHealthBar()
    {

        healthSlider.value = (float)currentHP / maxHP;
    }
    public StatusEffectManager statusEffectManager;
    public virtual void Start()
    {
        currentHP = maxHP;
        if (healthSlider != null)
            UpdateHealthBar();
    }

    public virtual void ReceiveDamage(int amount, CharacterBase attacker)
    {

        //modified damage
        int modifiedDamage = amount;

        foreach (var effect in statusEffectManager.GetAllEffects())
        {
            if (effect.effectName.StartsWith("DefenseDown_"))
            {
                if (int.TryParse(effect.effectName.Split('_')[1], out int downAmount))
                    modifiedDamage += downAmount; // Increase damage taken
            }
        }

        //Check confusion
        if (statusEffectManager.HasEffect("Confusion") && this is EnemyCharacter)
        {
            foreach (var effect in statusEffectManager.GetAllEffects())
            {
                if (effect.effectName.StartsWith("DefenseDown_"))
                {
                    if (int.TryParse(effect.effectName.Split('_')[1], out int downAmount))
                        modifiedDamage += downAmount; // Increase damage taken
                }
            }
            if (Random.value < 0.4f)
            {
                CombatNotificationUI.Instance?.Log($"{characterName}'s mimicry confused the attacker! The attack failed.");
                CombatSFXLibrary.Instance.PlayEvaded();
                return;
            }
        }

        // 🆕 Accuracy vs Evasion logic
        int hitChance = attacker.accuracy - GetModifiedEvasion();
        hitChance = Mathf.Clamp(hitChance, 10, 100); // Min 10%, max 100%

        if (Random.Range(0, 100) >= hitChance)
        {
            Debug.Log(characterName + " evaded the attack!");
            CombatNotificationUI.Instance.Log(characterName + " evaded the attack!");
            CombatSFXLibrary.Instance.PlayEvaded();
            TurnJumpController jumper = GetComponent<TurnJumpController>();
            if (jumper != null)
                StartCoroutine(jumper.JumpEvade(this is PlayerCharacter));
            return;
        }
        // Reflect damage
        if (statusEffectManager.HasEffect("Reflect"))
        {
            if (attacker is EnemyCharacter enemy && enemy.malwareType == MalwareType.Botnet)
            {
                SetReflectStrength(1.0f); // Full reflect
                Debug.Log("Firewall is very effective! Full reflect.");
                CombatNotificationUI.Instance.Log("Firewall is very effective! Full reflect.");
            }
            else
            {
                SetReflectStrength(0.25f);
                Debug.Log("Firewall: Will reflect 25% of attack.");
                CombatNotificationUI.Instance.Log("Firewall: Will reflect 25% of next attack.");
            }
            int reflected = Mathf.RoundToInt(amount * reflectStrength);
            attacker.ReceiveDamage(reflected, this);
            Debug.Log(characterName + " reflected " + reflected + " damage back to " + attacker.characterName);
            CombatNotificationUI.Instance.Log(characterName + " reflected " + reflected + " damage back to " + attacker.characterName);
            statusEffectManager.RemoveEffect("Reflect");
            CombatSFXLibrary.Instance.PlayReflect();
            return;
        }

        currentHP -= modifiedDamage;
        currentHP = Mathf.Max(0, currentHP);
        CombatNotificationUI.Instance.Log(characterName + " took " + modifiedDamage + " damage.");
        CombatSFXLibrary.Instance.PlayDamage();
        if (healthSlider != null)
        {
            UpdateHealthBar();
            StartCoroutine(Shake());
        }
        if (currentHP <= 0 && !hasDied)
        {
            hasDied = true;
            StartCoroutine(HandleDeath());
        }


    }

    public virtual void SureHitDamage(int amount, CharacterBase attacker)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log(characterName + " took " + amount + " damage.");
        CombatNotificationUI.Instance.Log(characterName + " took " + amount + " damage.");
        CombatSFXLibrary.Instance.PlayDamage();
        if (healthSlider != null)
        {
            UpdateHealthBar();
            StartCoroutine(Shake());
        }
        if (currentHP <= 0 && !hasDied)
        {
            hasDied = true;
            StartCoroutine(HandleDeath());
        }
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }
    public virtual IEnumerator TakeTurn()
    {
        statusEffectManager.TickDown();
        // Skip turn if stunned
        
        if (statusEffectManager.HasEffect("Stunned"))
        {
            Debug.Log(characterName + " is stunned and skips their turn!");
            CombatNotificationUI.Instance.Log(characterName + " is stunned and skips their turn!");
            statusEffectManager.RemoveEffect("Stunned");
            yield return new WaitForSeconds(1f);
            yield break;
        }

        // Player or Enemy turn logic continues here
        yield return null;
    }
    public GameObject targetIndicator;

    public void SetTargetIndicator(bool active)
    {
        if (targetIndicator != null)
            targetIndicator.SetActive(active);
    }

    //reflect mechanic
    private float reflectStrength = 0f;

    public void SetReflectStrength(float value)
    {
        reflectStrength = value;
    }

    public float GetReflectStrength()
    {
        return reflectStrength;
    }
    public int GetModifiedEvasion()
    {
        int baseEvasion = evasion;

        foreach (var effect in statusEffectManager.GetAllEffects())
        {
            if (effect.effectName.StartsWith("EvasionDown_"))
            {
                string[] split = effect.effectName.Split('_');
                if (int.TryParse(split[1], out int amount))
                    baseEvasion -= amount;
            }
        }

        return Mathf.Max(baseEvasion, 0); // Never negative
    }

    public IEnumerator Shake(float duration = 0.2f, float magnitude = 0.1f)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        Debug.Log("shake");
        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
    public void SetTurnIndicator(bool active)
    {
        if (turnHighlight != null)
            turnHighlight.SetActive(active);
    }

    public int GetAttackBonus()
    {
        int bonus = 0;
        foreach (var effect in statusEffectManager.GetAllEffects())
        {
            if (effect.effectName.StartsWith("AttackUp_"))
            {
                if (int.TryParse(effect.effectName.Split('_')[1], out int amount))
                    bonus += amount;
            }
        }
        return bonus;
    }
    //how to add
    //int totalDamage = baseDamage + user.GetAttackBonus();
    //target.ReceiveDamage(totalDamage, user);
    protected virtual IEnumerator HandleDeath()
    {
        CombatNotificationUI.Instance?.Log($"{characterName} has been defeated!");

        // Optional: wait for animation
        yield return new WaitForSeconds(0.1f);

        // Remove from turn order and target list
        CombatManager.Instance.UnregisterCharacter(this);

        // Disable visuals and collisions
        gameObject.SetActive(false);

        // Re-check win/lose conditions
        CombatManager.Instance.CheckCombatEnd();
    }


}
