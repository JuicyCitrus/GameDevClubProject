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
        CombatManager.Instance.ChangeTextMode(TextMode.fleeing);
        CombatManager.Instance.AdvanceTextMenu();
    }
}
