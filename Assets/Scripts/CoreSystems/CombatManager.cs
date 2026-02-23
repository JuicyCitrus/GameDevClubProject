using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    public struct CombatantInfo
    {
        public EntityDetails details;
        public Attack attackChoice;
    }

    public Vector3[] enemySpawnPoints;
    public Vector3[] allySpawnPoints;

    [Header("UI Elements")]
    public GameObject[] combatMenus;
    public TextMeshProUGUI textMenuText;

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
        int index = 0;

        // Spawn all enemies and add them to the combat list
        foreach (EntityDetails enemy in Combatants.currentLineup.enemies)
        {
            Vector3 spawnPoint = enemySpawnPoints[index];
            Instantiate(enemy.combatPrefab, spawnPoint, Quaternion.identity);

            enemiesInCombat.Add(new CombatantInfo
            {
                details = enemy,
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
                Instantiate(ally.combatPrefab, spawnPoint, Quaternion.identity);

                alliesInCombat.Add(new CombatantInfo
                {
                    details = ally,
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
        Debug.Log("Beginning turn...");

        // Set the current ally to the first one in the list at the start of the turn
        currentAlly = alliesInCombat[0];

        // Set the Action Menu to active so the player can choose an attack or other action
        ActivateMenu("ActionMenu");
    }

    private void EndOfTurn()
    {
        Debug.Log("Ending turn...");
    }
}
