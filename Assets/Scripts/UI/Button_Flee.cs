using UnityEngine;
using UnityEngine.UI;

public class Button_Flee : MonoBehaviour
{
    public Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(FleeCombat);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(FleeCombat);
    }

    private void FleeCombat()
    {
        CombatManager.Instance.textMode_fleeing = true;
        CombatManager.Instance.AdvanceTextMenu();
    }
}
