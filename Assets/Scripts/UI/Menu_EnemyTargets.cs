using TMPro;
using UnityEngine;

public class Menu_EnemyTargets : MonoBehaviour
{
    public GameObject[] buttons;
    public TextMeshProUGUI[] buttonText;

    private void OnEnable()
    {
        int index = 0;

        foreach(EntityDetails enemy in Combatants.currentLineup.enemies)
        {
            if (enemy != null && enemy.isDead == false)
            {
                // Turn on the button for this enemy and set its text to the enemy's name 
                buttons[index].SetActive(true);
                buttonText[index].text = enemy.entityName;

                // Set that button's target to this enemy
                if (buttons[index].GetComponent<Button_Target>() != null)
                    buttons[index].GetComponent<Button_Target>().thisEnemy = enemy.GetComponent<EntityDetails>();

                index++;
            }
        }
    }
}
