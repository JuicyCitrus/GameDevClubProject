using UnityEngine;

public class DialogueTrigger : Interactable
{
    public DialogueSystem dialogueSystem;

    public override void Interact()
    {
        dialogueSystem.ActivateDialogue();
    }
}
