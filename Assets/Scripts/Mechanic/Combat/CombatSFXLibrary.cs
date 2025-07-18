using UnityEngine;

public class CombatSFXLibrary : MonoBehaviour
{
    public static CombatSFXLibrary Instance;

    [Header("SFX Clips")]
    public AudioClip buffSFX;
    public AudioClip debuffSFX;
    public AudioClip evadedSFX;
    public AudioClip reflectSFX;
    public AudioClip receiveDamageSFX;
    public AudioClip summonSFX;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayBuff() => SFXManager.Instance.PlaySFX(buffSFX);
    public void PlayDebuff() => SFXManager.Instance.PlaySFX(debuffSFX);
    public void PlayEvaded() => SFXManager.Instance.PlaySFX(evadedSFX);
    public void PlayReflect() => SFXManager.Instance.PlaySFX(reflectSFX);
    public void PlayDamage() => SFXManager.Instance.PlaySFX(receiveDamageSFX);
    public void PlaySummon() => SFXManager.Instance.PlaySFX(summonSFX);
}
