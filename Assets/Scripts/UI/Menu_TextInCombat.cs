using UnityEngine;

public class Menu_TextInCombat : MonoBehaviour
{
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Enable input and bind it to the proper function in the combat manager
        controls.UI.Enable();
        controls.UI.Submit.performed += ctx => CombatManager.Instance.AdvanceTextMenu();
    }

    private void OnDisable()
    {
        controls.UI.Submit.performed -= ctx => CombatManager.Instance.AdvanceTextMenu();
        controls.UI.Disable();
    }
}
