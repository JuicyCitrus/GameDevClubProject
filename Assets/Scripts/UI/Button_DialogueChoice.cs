using UnityEngine;
using UnityEngine.UI;

public class Button_DialogueChoice : MonoBehaviour
{
    public Button button;
    public DialogueEvent nextPartOfConversation;

    private void OnEnable()
    {
        button.onClick.AddListener(MakeDialogueChoice);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(MakeDialogueChoice);
    }

    private void MakeDialogueChoice()
    {
        DialogueSystem.Instance.dialogue = nextPartOfConversation;
        DialogueSystem.Instance.ActivateDialogue();
    }
}
