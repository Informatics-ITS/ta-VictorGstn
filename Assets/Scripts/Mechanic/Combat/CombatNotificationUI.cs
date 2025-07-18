using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatNotificationUI : MonoBehaviour
{
    public static CombatNotificationUI Instance;

    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject messagePrefab; // A TextMeshProUGUI prefab
    [SerializeField] private Color evenColor = new Color(1f, 1f, 1f);          // White
    [SerializeField] private Color oddColor = new Color(0.8f, 0.8f, 0.8f);    // Light gray


    private readonly Queue<GameObject> messageQueue = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Log(string message)
    {
        GameObject msgObj = Instantiate(messagePrefab, contentRoot);
        TextMeshProUGUI text = msgObj.GetComponent<TextMeshProUGUI>();
        text.text = message;

        // NEW: alternate color based on queue count
        text.color = (messageQueue.Count % 2 == 0) ? evenColor : oddColor;

        messageQueue.Enqueue(msgObj);

    }
}
