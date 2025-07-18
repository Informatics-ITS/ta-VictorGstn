using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)] public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Script")]
public class DialogueScript : ScriptableObject
{
    public DialogueLine[] lines;
}
