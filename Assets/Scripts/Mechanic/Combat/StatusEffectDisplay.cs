using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class StatusEffectDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI statusEffectText;
    [SerializeField] private float blinkDuration = 0.5f;
    private Coroutine displayRoutine;

    private List<string> statusEffects = new List<string>();
    private int currentEffectIndex = 0;
   // private bool isBlinking = false;

    private void Start()
    {
        statusEffectText.gameObject.SetActive(false); // Hide initially
    }

    public void ShowStatusEffects(List<string> effects)
    {
        statusEffects = effects;

        if (statusEffects.Count > 0)
        {
            if (displayRoutine != null)
                StopCoroutine(displayRoutine);
            displayRoutine = StartCoroutine(DisplayEffects());
        }
        else
        {
            statusEffectText.text = "";
            statusEffectText.gameObject.SetActive(false);

            if (displayRoutine != null)
            {
                StopCoroutine(displayRoutine);
                displayRoutine = null;
            }
        }
    }

    private IEnumerator DisplayEffects()
    {
        while (true && statusEffects.Count>0)
        {
            statusEffectText.text = statusEffects[currentEffectIndex];
            statusEffectText.gameObject.SetActive(true); // Show text

            // Blink effect
          //  isBlinking = true;
            yield return BlinkEffect();

            // Wait for the text duration before switching to next status
            yield return new WaitForSeconds(blinkDuration);

            // Move to the next effect in the list
            if (statusEffects.Count > 0)
                currentEffectIndex = (currentEffectIndex + 1) % statusEffects.Count;
            else yield break;
            // Wait a little before showing the next effect
            yield return new WaitForSeconds(0.2f); // Delay before the next effect
        }
    }

    private IEnumerator BlinkEffect()
    {
        //Debug.Log("blink");
        float elapsedTime = 0f;
        Color originalColor = statusEffectText.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < blinkDuration)
        {
            float t = Mathf.PingPong(elapsedTime / blinkDuration, 1);
            statusEffectText.color = Color.Lerp(originalColor, transparentColor, t);
            //statusEffectText.rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(elapsedTime * moveSpeed) * moveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        statusEffectText.color = originalColor;
    }
}
