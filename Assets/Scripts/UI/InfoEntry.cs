using UnityEngine;
[CreateAssetMenu(menuName = "InfoEntry")]
public class InfoEntry : ScriptableObject
{
    public string entryName;
    [TextArea(5, 15)]
    public string infoText;
}
