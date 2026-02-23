using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public int attackTextIndex = 0;

    // Different modes for text menu
    public bool textMode_endOfTurnAttacks = false;
    public bool textMode_fleeing = false;

    public List<CombatantInfo> enemiesInCombat = new List<CombatantInfo>();
    public List<CombatantInfo> alliesInCombat = new List<CombatantInfo>();
    public CombatantInfo currentAlly;
    List<AttackInSequence> allAttacksSorted = new List<AttackInSequence>();

    private bool attackKilled = false;
    private int enemiesKilled = 0;

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
        // End combat if all enemies are dead
        if(enemiesKilled >= enemiesInCombat.Count)
        {
            BootstrapSceneManager.Instance.LoadNewScene(SceneManager.GetActiveScene().name, BootstrapSceneManager.Instance.previousSceneName);
        }

        // Set the current ally to the first one in the list at the start of the turn
        currentAlly = alliesInCombat[0];

        // Set the Action Menu to active so the player can choose an attack or other action
        ActivateMenu("ActionMenu");
    }

    public void ChoiceMade()
    {
        int index = 0;

        // Find the entry in alliesInCombat that matches currentAlly
        foreach (var ally in alliesInCombat)
        {
            if (alliesInCombat[index].details.name == currentAlly.details.name)
            {
                break;
            }

            index++;
        }

        // If the next entry in the alliesInCombat list isn't null, set up the choice sequence for that next ally
        if (index + 1 < alliesInCombat.Count)
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
        // Go through all end of turn attacks via the text menu
        if (textMode_endOfTurnAttacks)
        {
            AdvanceEndOfTurnCombat();
        }
        // Display a fleeing message via the text menu and then go back to the previous scene.
        else if (textMode_fleeing)
        {
            // Set text if not set yet
            if (textMenuText.text != fleeMessage)
            {
                ActivateTextMenu(fleeMessage);
            }

            // Change scene if text has been set
            else if (textMenuText.text == fleeMessage)
            {
                BootstrapSceneManager.Instance.LoadNewScene(SceneManager.GetActiveScene().name, BootstrapSceneManager.Instance.previousSceneName);
            }
        }
    }

    public void AdvanceEndOfTurnCombat()
    {
        // If the index exceeds the list of attacks to go through at the end of the turn, begin the next turn instead
        if (attackTextIndex >= allAttacksSorted.Count)
        {
            attackTextIndex = 0;
            textMode_endOfTurnAttacks = false;
            BeginTurn();
        }
        // Otherwise display the next attack via the text menu and carry it out through EntityDetails health mechanics
        else
        {
            // Only perform the behavior if the attacker hasn't died in combat yet
            if (!allAttacksSorted[attackTextIndex].attacker.isDead && !attackKilled)
            {
                // Text menu display
                ActivateTextMenu(allAttacksSorted[attackTextIndex].attacker.entityName + " used " +
                    allAttacksSorted[attackTextIndex].attackBeingPerformed.name + " on "
                    + allAttacksSorted[attackTextIndex].attackTarget.entityName + ".");

                // Health mechanics
                allAttacksSorted[attackTextIndex].attackTarget.AdjustHealth(
                    -allAttacksSorted[attackTextIndex].attacker.baseAttack * allAttacksSorted[attackTextIndex].attackBeingPerformed.baseDamage);

                // If that killed it, set the attackKilled bool to true so the next text message can confirm the kill
                if(allAttacksSorted[attackTextIndex].attackTarget.currentHealth <= 0)
                {
                    attackKilled = true;
                    enemiesKilled++;
                }
                // Otherwise move on to the next attack
                else
                {
                    attackTextIndex++;
                }
            }
            // If the attack killed something, confirm it via the text menu and move on to the next attack
            else if (attackKilled)
            {
                attackKilled = false;
                ActivateTextMenu(allAttacksSorted[attackTextIndex].attackTarget.entityName + " was killed.");

                // Move on to the next attack
                attackTextIndex++;
            }
            // In any other case, move on to the next attack
            else
            {
                attackTextIndex++;
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

        // Clear the sorted attacks list
        allAttacksSorted.Clear();
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

        // Walk through the combat in the text menu
        textMode_endOfTurnAttacks = true;
        AdvanceTextMenu();
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
