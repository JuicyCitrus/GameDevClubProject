using UnityEngine;
using UnityEngine.UI;

public class Button_Attack : MonoBehaviour
{
    public Button button;
    public Attack attack;

    private void OnEnable()
    {
        button.onClick.AddListener(SelectAttack);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SelectAttack);
    }

    private void SelectAttack()
    {
        CombatManager.Instance.currentAlly.attackChoice = attack;
    }
}
