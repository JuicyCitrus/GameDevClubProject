using TMPro;
using UnityEngine;

public class Menu_EnemyTargets : MonoBehaviour
{
    public GameObject[] buttons;
    public TextMeshProUGUI[] buttonText;
    public EnemyDetails[] enemyCombatants;

    private void Start()
    {
        int index = 0;

        // Get all combatants for the scene and populate the menu with their buttons and names
        foreach (GameObject enemy in Combatants.currentLineup.enemies)
        {
            if (enemy != null)
            {   // Turn on the button for this enemy and set its text to the enemy's name             
                buttons[index].SetActive(true);
                if(enemy.GetComponent<EnemyDetails>() != null)
                    buttonText[index].text = enemy.GetComponent<EnemyDetails>().enemyName;

                // Increment index
                index++;
            }
        }

        // Deactivate this menu
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        foreach(EnemyDetails enemy in enemyCombatants)
        {
            if (enemy != null)
            {
                int index = System.Array.IndexOf(enemyCombatants, enemy);
                buttons[index].SetActive(true);
                buttonText[index].text = enemy.enemyName;
            }
        }
    }
}
