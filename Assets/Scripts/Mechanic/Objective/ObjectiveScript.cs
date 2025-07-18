using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "Quest/Objective Script")]
public class ObjectiveScript : ScriptableObject
{
    [TextArea(2, 4)]
    public string[] objectives;
}
