using UnityEngine;

public class DialogueTrigger : Interactable
{
    public DialogueEvent dialogueEvent;

    public override void Interact(GameObject player)
    {

        // Set the dialogue system's dialogue event to the scriptable object specified in this trigger, then activate the dialogue system
        DialogueSystem.Instance.dialogue = dialogueEvent;
        DialogueSystem.Instance.ActivateDialogue();

        // Fill the enemy line up with the one in this scriptable object if it exists
        if (dialogueEvent.enemyLineup != null)
        {
            DialogueSystem.Instance.combatEnemies = dialogueEvent.enemyLineup;
            DialogueSystem.Instance.enterCombatAfterDialogue = true;
        }
    }
}
