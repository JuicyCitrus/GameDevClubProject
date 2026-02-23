using TMPro;
using UnityEngine;

public class Menu_EnemyTargets : MonoBehaviour
{
    public GameObject[] buttons;
    public TextMeshProUGUI[] buttonText;

    private void OnEnable()
    {
        int index = 0;

        foreach(CombatManager.CombatantInfo enemy in CombatManager.Instance.enemiesInCombat)
        {
            if (enemy != null && enemy.details.isDead == false)
            {
                Debug.Log(enemy.details.name + " is " + enemy.details.isDead);
                // Turn on the button for this enemy and set its text to the enemy's name 
                buttons[index].SetActive(true);
                buttonText[index].text = enemy.details.entityName;

                // Set that button's target to this enemy
                if (buttons[index].GetComponent<Button_Target>() != null)
                    buttons[index].GetComponent<Button_Target>().thisEnemy = enemy.details;

                index++;
            }
            else
            {
                buttons[index].SetActive(false);
            }
        }
    }
}
