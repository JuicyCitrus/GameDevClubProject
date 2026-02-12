using TMPro;
using UnityEngine;

public class Menu_EnemyTargets : MonoBehaviour
{
    public GameObject[] buttons;
    public TextMeshProUGUI[] buttonText;
    public EnemyDetails[] enemyCombatants;

    private void Start()
    {
        // Get all combatants for the scene and populate the menu with their buttons and names
        foreach (EnemyDetails enemy in Combatants.enemyCombatants)
        {
            if (enemy != null)
            {
                int index = System.Array.IndexOf(Combatants.enemyCombatants, enemy);
                buttons[index].SetActive(true);
                buttonText[index].text = enemy.enemyName;
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
