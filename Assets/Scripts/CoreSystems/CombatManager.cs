using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    public class CombatantInfo
    {
        public EntityDetails details;
        public Attack attackChoice;
        public EntityDetails attackTarget;
    }

    public class AttackInSequence
    {
        public EntityDetails attacker;
        public EntityDetails attackTarget;
        public Attack attackBeingPerformed;
        public int totalPriority;
    }

    public Vector3[] enemySpawnPoints;
    public Vector3[] allySpawnPoints;

    [Header("UI Elements")]
    public GameObject[] combatMenus;
    public TextMeshProUGUI textMenuText;

    [Header("Text Menu Lines")]
    public string fleeMessage = "Fleeing combat...";

    // Different modes for text menu
    public bool textMode_endOfTurnAttacks = false;
    public bool textMode_fleeing = false;

    public List<CombatantInfo> enemiesInCombat = new List<CombatantInfo>();
    public List<CombatantInfo> alliesInCombat = new List<CombatantInfo>();
    public CombatantInfo currentAlly;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Set text modes back to false
        textMode_endOfTurnAttacks = false;
        textMode_fleeing = false;

        int index = 0;

        // Spawn all enemies and add them to the combat list
        foreach (EntityDetails enemy in Combatants.currentLineup.enemies)
        {
            Vector3 spawnPoint = enemySpawnPoints[index];
            GameObject newEnemy = Instantiate(enemy.combatPrefab, spawnPoint, Quaternion.identity);

            enemiesInCombat.Add(new CombatantInfo
            {
                details = newEnemy.GetComponent<EntityDetails>(),
                attackChoice = null // No attack chosen at the start of combat
            });

            index++;
        }

        // Spawn the player's party and add them to the combat list
        if (PartyManager.Instance != null)
        {
            index = 0;
            foreach (EntityDetails ally in PartyManager.Instance.partyMembers)
            {
                Vector3 spawnPoint = allySpawnPoints[index];
                GameObject newAlly = Instantiate(ally.combatPrefab, spawnPoint, Quaternion.identity);

                alliesInCombat.Add(new CombatantInfo
                {
                    details = newAlly.GetComponent<EntityDetails>(),
                    attackChoice = null // No attack chosen at the start of combat
                });

                index++;
            }
        }

        BeginTurn();
    }

    public void ActivateMenu(string menuName)
    {
        foreach (GameObject menu in combatMenus)
        {
            menu.SetActive(menu.name == menuName);
        }
    }

    public void ActivateTextMenu(string text)
    {
        textMenuText.text = text;
        ActivateMenu("TextMenu");
    }

    private void BeginTurn()
    {
        // Set the current ally to the first one in the list at the start of the turn
        currentAlly = alliesInCombat[0];

        // Set the Action Menu to active so the player can choose an attack or other action
        ActivateMenu("ActionMenu");
    }

    public void ChoiceMade()
    {
        int index = 0; 

        // Find the entry in alliesInCombat that matches currentAlly
        foreach(var ally in alliesInCombat)
        {
            if (alliesInCombat[index].details.name == currentAlly.details.name)
            {
                break;
            }

            index++;
        }

        // If the next entry in the alliesInCombat list isn't null, set up the choice sequence for that next ally
        if(index + 1 < alliesInCombat.Count)
        {
            if (alliesInCombat[index + 1].details != null)
            {
                currentAlly = alliesInCombat[index + 1];
                Debug.Log("Resetting actions with index " + currentAlly.details.entityName);
                ActivateMenu("ActionMenu");
            }
        }
        else
        {
            EndOfTurn();
        }
    }

    public void AdvanceTextMenu()
    {
        if (textMode_endOfTurnAttacks)
        {
            Debug.Log("Advancing end of turn attacks");
        }
        else if (textMode_fleeing)
        {
            // Set text if not set yet
            if(textMenuText.text != fleeMessage)
            {
                ActivateTextMenu(fleeMessage);
            }

            // Change scene if text has been set
            else if(textMenuText.text == fleeMessage)
            {
                BootstrapSceneManager.Instance.LoadNewScene(SceneManager.GetActiveScene().name, BootstrapSceneManager.Instance.previousSceneName);
            }
        }
    }

    private void EndOfTurn()
    {
        // Create new list of attacks in Sequence and order them based on the attack's priority
        List<AttackInSequence> allAttacks = new List<AttackInSequence>();

        // Get all allied attacks
        int index = 0;
        foreach (var ally in alliesInCombat)
        {
            AttackInSequence currentAttack = new AttackInSequence();
            currentAttack.attacker = alliesInCombat[index].details;
            currentAttack.attackTarget = alliesInCombat[index].attackTarget;
            currentAttack.attackBeingPerformed = alliesInCombat[index].attackChoice;
            currentAttack.totalPriority = currentAttack.attacker.speed * currentAttack.attackBeingPerformed.priority;
            allAttacks.Add(currentAttack);

            index++;
        }

        // Get all enemy attacks
        ChooseEnemyAttacks();
        index = 0;
        foreach (var ally in enemiesInCombat)
        {
            AttackInSequence currentAttack = new AttackInSequence();
            currentAttack.attacker = enemiesInCombat[index].details;
            currentAttack.attackTarget = enemiesInCombat[index].attackTarget;
            currentAttack.attackBeingPerformed = enemiesInCombat[index].attackChoice;
            currentAttack.totalPriority = currentAttack.attacker.speed * currentAttack.attackBeingPerformed.priority;
            allAttacks.Add(currentAttack);

            index++;
        }

        // Make a new attack list for the sorted attacks
        List<AttackInSequence> allAttacksSorted = new List<AttackInSequence>();
        int fastestIndex = 0;

        // Find the fastest attacks one by one and add them to the new list while removing them from the old
        while (allAttacks.Count != 0)
        {
            AttackInSequence fastestAttack = new AttackInSequence();

            for(int i = 0; i < allAttacks.Count; i++)
            {
                // The first attack found will be the fastest by default until it is overwritten
                if(i == 0)
                {
                    fastestAttack = allAttacks[i];
                }

                // If the attack in the list is faster than the current fastest attack, overwrite it
                if (allAttacks[i].totalPriority > fastestAttack.totalPriority)
                {
                    fastestAttack = allAttacks[i];
                    fastestIndex = i;
                }
            }

            allAttacksSorted.Add(fastestAttack);
            allAttacks.Remove(allAttacks[fastestIndex]);
        }

        // Using the sorted list, perform all attacks
        for(int i = 0; i < allAttacksSorted.Count; i++)
        {
            if (allAttacksSorted[i].attacker.isDead)
            {
                continue;
            }
            allAttacksSorted[i].attackTarget.AdjustHealth(-allAttacksSorted[i].attacker.baseAttack * allAttacksSorted[i].attackBeingPerformed.baseDamage);
        }

        // Begin the next turn
        BeginTurn();
    }

    private void ChooseEnemyAttacks()
    {
        for(int i = 0; i < enemiesInCombat.Count; i++)
        {
            enemiesInCombat[i].attackTarget = alliesInCombat[Random.Range(0, alliesInCombat.Count - 1)].details;
            enemiesInCombat[i].attackChoice = enemiesInCombat[i].details.attacks[Random.Range(0, enemiesInCombat[i].details.attacks.Length - 1)];
        }
    }
}
