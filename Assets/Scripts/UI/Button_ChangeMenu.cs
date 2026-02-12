using UnityEngine;
using UnityEngine.UI;

public class Button_ChangeMenu : MonoBehaviour
{
    public Button button;
    public GameObject menuToOpen;
    public GameObject menuToClose;

    private void OnEnable()
    {
        button.onClick.AddListener(OpenMenu);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OpenMenu);
    }

    private void OpenMenu()
    {
        menuToOpen.SetActive(true);
        menuToClose.SetActive(false);
    }
}
