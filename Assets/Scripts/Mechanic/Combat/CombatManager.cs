using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    private List<CharacterBase> turnOrder = new List<CharacterBase>();
    private int currentTurnIndex = 0;

    [SerializeField] private List<CharacterBase> playerCharacters;
    [SerializeField] private List<CharacterBase> enemyCharacters;

    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private CombatResultManager combatResultManager;
    public GameObject ZombiePrefab => zombiePrefab;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("Setup combat");
        SetupCombat();
        Debug.Log("Start combat loop");
        StartCoroutine(CombatLoop());
    }
    
    void SetupCombat()
    {
        turnOrder.Clear();

        foreach (var p in playerCharacters)
            turnOrder.Add(p);

        foreach (var e in enemyCharacters)
        {
            turnOrder.Add(e);
        }
    }

    IEnumerator CombatLoop()
    {
        while (true)
        {
            Debug.Log("Looping");
            CharacterBase currentChar = turnOrder[currentTurnIndex];
            if (currentChar.IsDead())
            {
                currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
                Debug.Log("continue");
                continue;
            }
            currentChar.SetTurnIndicator(true);
            yield return StartCoroutine(currentChar.TakeTurn());
            Debug.Log("Finished turn for " + currentChar.characterName);
            currentChar.SetTurnIndicator(false);
            yield return new WaitForSeconds(0.25f);

            CheckCombatEnd();
            currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        }
    }


    public List<Transform> GetAvailableEnemySpawns()
    {
        List<Transform> available = new();
        foreach (Transform point in enemySpawnPoints)
        {
            bool occupied = enemyCharacters.Exists(e => Vector2.Distance(e.transform.position, point.position) < 0.6f);
            if (!occupied)
                available.Add(point);
        }
        return available;
    }
    public void RegisterEnemy(EnemyCharacter enemy)
    {
        if (!enemyCharacters.Contains(enemy))
            enemyCharacters.Add(enemy);

        if (!turnOrder.Contains(enemy))
            turnOrder.Insert(currentTurnIndex + 1, enemy); // optional: insert after current turn

        enemy.SetTurnIndicator(false); // just in case
    }



    public List<CharacterBase> GetEnemies(CharacterBase user)
    {
        return (user is PlayerCharacter) ? enemyCharacters : playerCharacters;
    }

    public List<CharacterBase> GetAllies(CharacterBase user)
    {
        return (user is PlayerCharacter) ? playerCharacters : enemyCharacters;
    }

    public List<CharacterBase> GetTargets(CharacterBase user, TargetingType type)
    {
        if (type == TargetingType.Enemy)
            return GetEnemies(user).FindAll(c => !c.IsDead());

        if (type == TargetingType.Ally)
            return GetAllies(user).FindAll(c => !c.IsDead());

        if (type == TargetingType.Self)
            return new List<CharacterBase> { user };

        return new List<CharacterBase>();
    }

    public void EndCombat()
    {
        CombatResultManager.Instance.ShowVictory();
    }
    public void CheckCombatEnd()
    {
        bool allEnemiesDead = enemyCharacters.TrueForAll(e => e.IsDead());
        bool allPlayersDead = playerCharacters.TrueForAll(p => p.IsDead());

        if (allEnemiesDead)
        {
            Debug.Log("Combat won!");
            CombatNotificationUI.Instance?.Log("All enemies defeated!");
            EndCombat(); // fades back to overworld
        }
        else if (allPlayersDead)
        {
            Debug.Log("Combat lost...");
            CombatNotificationUI.Instance?.Log("All party members defeated...");
            GameOver();
        }
    }
    public void GameOver()
    {
        CombatResultManager.Instance.ShowDefeat();
    }
    public CharacterBase GetRandomTarget(CharacterBase user, TargetingType type)
    {
        List<CharacterBase> candidates = new();

        switch (type)
        {
            case TargetingType.Enemy:
                candidates = GetEnemies(user).FindAll(c => !c.IsDead());
                break;

            case TargetingType.Ally:
                candidates = GetAllies(user).FindAll(c => !c.IsDead() && c != user);
                break;

            case TargetingType.Self:
                return user;

        }

        if (candidates.Count == 0) return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    public void UnregisterCharacter(CharacterBase character)
    {
        if (turnOrder.Contains(character))
            turnOrder.Remove(character);

        if (character is PlayerCharacter && playerCharacters.Contains(character))
            playerCharacters.Remove(character);

        if (character is EnemyCharacter && enemyCharacters.Contains(character))
            enemyCharacters.Remove(character);
    }

}
