using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingType
{
    Enemy,
    Ally,
    Self
}

public class TargetingManager : MonoBehaviour
{
    public static TargetingManager Instance;

    private List<CharacterBase> currentTargets = new List<CharacterBase>();
    private int currentIndex = 0;

    private void Awake() => Instance = this;

    public void StartTargeting(CharacterBase user, TargetingType type, System.Action<CharacterBase> onTargetSelected)
    {
        Debug.Log("Start targeting (" + type + ")");
        HighlightNone();
        currentTargets.Clear();

        // Get targets based on type
        if (type == TargetingType.Enemy)
        {
            currentTargets = new List<CharacterBase>(CombatManager.Instance.GetEnemies(user));
        }
        else if (type == TargetingType.Ally)
        {
            currentTargets = new List<CharacterBase>(CombatManager.Instance.GetAllies(user));
        }
        else if (type == TargetingType.Self)
        {
            currentTargets.Add(user);
        }

        if (currentTargets.Count == 0)
        {
            Debug.LogWarning("No targets available for targeting type: " + type);
            return;
        }

        currentIndex = 0;
        HighlightCurrent();
        StartCoroutine(HandleInput(onTargetSelected));
    }

    private void HighlightCurrent()
    {
        for (int i = 0; i < currentTargets.Count; i++)
            currentTargets[i].SetTargetIndicator(i == currentIndex);
    }

    private void HighlightNone()
    {
        foreach (var c in currentTargets)
            c.SetTargetIndicator(false);
    }

    private IEnumerator HandleInput(System.Action<CharacterBase> onTargetSelected)
    {
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex = (currentIndex - 1 + currentTargets.Count) % currentTargets.Count;
                HighlightCurrent();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = (currentIndex + 1) % currentTargets.Count;
                HighlightCurrent();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("target selected");
                onTargetSelected?.Invoke(currentTargets[currentIndex]);
                HighlightNone();
                yield break;
            }

            yield return null;
        }
    }
}
