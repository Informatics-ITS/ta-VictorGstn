using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sfxClip;

    public void PlaySFX()
    {
        Debug.Log("playSFX");
        audioSource.PlayOneShot(sfxClip);
    }
}
