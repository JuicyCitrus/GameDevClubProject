using TMPro;
using UnityEngine;

public class Menu_Attacks : MonoBehaviour
{
    [System.Serializable] public struct AttackButton
    {
        public GameObject buttonGameObject;
        public Button_Attack buttonScript;
        public TextMeshProUGUI buttonText;
    }

    public AttackButton[] attackChoiceButtons;

    private void OnEnable()
    {
        int index = 0;

        // Fill as many buttons as possile with attacks from the current ally
        foreach (var button in attackChoiceButtons)
        {
            if (index > (CombatManager.Instance.currentAlly.details.attacks.Length - 1))
            {
                break;
            }
            if (CombatManager.Instance.currentAlly.details.attacks[index] != null)
            {
                attackChoiceButtons[index].buttonGameObject.SetActive(true);
                attackChoiceButtons[index].buttonScript.attack = CombatManager.Instance.currentAlly.details.attacks[index];
                attackChoiceButtons[index].buttonText.text = CombatManager.Instance.currentAlly.details.attacks[index].name;
            }

            index++;
        }

        // Turn off the rest of the buttons
        for(int i = index; i < attackChoiceButtons.Length; i++)
        {
            attackChoiceButtons[i].buttonGameObject.SetActive(false);
        }

    }
}
