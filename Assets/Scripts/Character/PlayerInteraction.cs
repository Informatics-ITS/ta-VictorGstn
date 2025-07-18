using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public string interactionKey = "z";
    private List<Interactable> nearbyInteractables = new List<Interactable>();
    private Interactable closestInteractable;

    void Update()
    {
        UpdateClosestInteractable();

        if (closestInteractable != null)
        {
            if (DialogueManager.Instance != null && !DialogueManager.Instance.CanInteract())
                return; // Dialogue is active, so ignore other interactions
            if (Input.GetKeyDown(interactionKey))
            {
                closestInteractable.Interact();
            }
        }
    }

    private void UpdateClosestInteractable()
    {
        float minDistance = Mathf.Infinity;
        Interactable nearest = null;

        foreach (Interactable interactable in nearbyInteractables)
        {
            if (interactable == null) continue;

            float dist = Vector2.Distance(transform.position, interactable.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = interactable;
            }
        }

        // Update floating prompt display
        foreach (var obj in nearbyInteractables)
        {
            if (obj == null) continue;
            obj.ShowPrompt(obj == nearest);
        }

        closestInteractable = nearest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.ShowPrompt(false);
            nearbyInteractables.Remove(interactable);
            if (closestInteractable == interactable)
            {
                closestInteractable = null;
            }
        }
    }
}
