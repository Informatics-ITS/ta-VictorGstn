using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameObject floatingPrompt;
    public InteractionType interactionType;

    protected virtual void Start()
    {
        if (floatingPrompt != null)
            floatingPrompt.SetActive(false);
    }

    public abstract void Interact();

    public void ShowPrompt(bool state)
    {
        if (floatingPrompt != null)
            floatingPrompt.SetActive(state);
    }
}

