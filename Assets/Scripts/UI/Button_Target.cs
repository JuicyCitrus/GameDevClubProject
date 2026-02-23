using UnityEngine;
using UnityEngine.UI;

public class Button_Target : MonoBehaviour
{
    public Button button;
    public EntityDetails thisEnemy;

    private void OnEnable()
    {
        button.onClick.AddListener(SelectTarget);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SelectTarget);
    }

    private void SelectTarget()
    {
        CombatManager.Instance.currentAlly.attackTarget = thisEnemy;
        Debug.Log("Player will use " + CombatManager.Instance.currentAlly.attackChoice + " on " + CombatManager.Instance.currentAlly.attackTarget);
        CombatManager.Instance.ChoiceMade();
    }
}
