using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    // public Image portraitImage;

    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private bool dialogueActive = false;

    private float interactionCooldown = 0.2f;
    private float cooldownTimer = 0f;
    private float lineSkipDelay = 0.25f; // How long to wait before allowing Z to proceed
    private float lineTimer = 0f;
    private System.Action onDialogueComplete;
    [SerializeField] private SFXPlayer sfx;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueUI.SetActive(false);
    }

    public void StartDialogue(DialogueScript script, System.Action onComplete = null)
    {
        dialogueQueue.Clear();
        foreach (var line in script.lines)
            dialogueQueue.Enqueue(line);

        dialogueUI.SetActive(true);
        dialogueActive = true;
        lineTimer = lineSkipDelay;
        onDialogueComplete = onComplete;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            Debug.Log("dialogue count: " + dialogueQueue.Count);
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueQueue.Dequeue();
        nameText.text = line.speakerName;
        dialogueText.text = line.text;
        sfx.PlaySFX();
        lineTimer = lineSkipDelay;
    }

    void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        if (!dialogueActive) return;

        if (lineTimer > 0f)
        {
            lineTimer -= Time.deltaTime;
            return; // Don't allow Z press yet
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Next Line");
            DisplayNextLine();
        }
    }

    private void EndDialogue()
    {   
        Debug.Log("End Dialogue");
        dialogueUI.SetActive(false);
        dialogueActive = false;
        cooldownTimer = interactionCooldown;
        // Unlock controls or send event if needed
        onDialogueComplete?.Invoke();
        
        onDialogueComplete = null;
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    public bool CanInteract()
    {
        return !dialogueActive && cooldownTimer <= 0f;
    }

}
