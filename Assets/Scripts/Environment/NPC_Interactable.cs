using UnityEngine;

public class NPC : Interactable
{
    public DialogueScript dialogueScript;
    
    public DialogueScript lockedDialogueScript;

    [SerializeField] private int requiredObjectiveIndex = 0;
    public bool advanceObjectiveAfterDialogue = false;

    private bool hasSpoken = false;
    public bool repeatable = true;
    public bool lock_repeatable = true;
    private bool lock_hasSpoken = false;
    private void Awake()
    {
        interactionType = InteractionType.Dialogue;
    }

    public override void Interact()
    {
        

        int currentObjective = QuestManager.Instance.currentObjectiveIndex;

        if (currentObjective >= requiredObjectiveIndex)
        {
            if (!lock_repeatable && lock_hasSpoken)
                return;
            DialogueManager.Instance.StartDialogue(lockedDialogueScript);
            lock_hasSpoken = true;
            OnDialogueComplete();
        }
        else if (dialogueScript != null)
        {
            if (!repeatable && hasSpoken)
                return;
            DialogueManager.Instance.StartDialogue(dialogueScript);
            hasSpoken = true;
            OnDialogueComplete();
        }
        
    }

    public void OnDialogueComplete()
    {
        if (advanceObjectiveAfterDialogue)
        {
            QuestManager.Instance.AdvanceObjective();
        }
    }
}


