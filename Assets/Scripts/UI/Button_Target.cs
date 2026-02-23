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
        CombatData.playerEnemyChoice = thisEnemy;
        Debug.Log("Player will use " + CombatData.playerAttackChoice.name + " on " + CombatData.playerEnemyChoice.entityName);
    }
}
