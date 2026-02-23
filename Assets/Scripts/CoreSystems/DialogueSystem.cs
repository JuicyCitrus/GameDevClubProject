using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSegment
    {
        public string dialogueLine;
        public Image characterImage;
        public string characterName;
        public bool isLeftSide;
    }

    [Header("Dialogue Settings")]
    public DialogueSegment[] dialogueSegments;

    [Header("Combat Settings")]
    public bool enterCombatAfterDialogue = false;
    public string combatSceneName;
    public EnemyLineup combatEnemies;

    [Header("UI Elements")]
    public Canvas dialogueCanvas;
    public Image leftSideSprite;
    public Image rightSideSprite;
    public TextMeshProUGUI leftNamecard;
    public TextMeshProUGUI rightNamecard;
    public TextMeshProUGUI dialogueText;

    private Controls controls;
    private int currentIndex = 0;

    private void Awake()
    {
        controls = new Controls();
    }

    public void ActivateDialogue()
    {
        // Reset dialogue index so it doesn't immediately end when Advance Dialogue is called
        currentIndex = 0;

        // Stop time
        Time.timeScale = 0f;

        // Enable dialogue UI and input
        controls.UI.Enable();
        controls.UI.Submit.performed += ctx => AdvanceDialogue();

        // Turn on the visuals
        dialogueCanvas.enabled = true;

        // Progress first step of dialogue
        AdvanceDialogue();
    }

    public void DeactivateDialogue()
    {
        // Disable controls and UI visuals
        controls.UI.Submit.performed -= ctx => AdvanceDialogue();
        controls.UI.Disable();
        dialogueCanvas.enabled = false;

        // Resume time
        Time.timeScale = 1f;
    }

    public void InputAdvanceDialogue()
    {
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        // End dialogue if no more segments
        if (currentIndex >= dialogueSegments.Length)
        {
            DeactivateDialogue();
            if (enterCombatAfterDialogue)
            {
                // Fill the Combatants static class with the enemies for the next combat scene, then load it
                Combatants.currentLineup = combatEnemies;

                // Load the scene
                EnterCombat(combatSceneName);
            }
            return;
        }

        // Update dialogue UI
        if (dialogueSegments[currentIndex].isLeftSide)
        {
            leftSideSprite.sprite = dialogueSegments[currentIndex].characterImage.sprite;
            leftNamecard.text = dialogueSegments[currentIndex].characterName;
        }
        else
        {
            rightSideSprite.sprite = dialogueSegments[currentIndex].characterImage.sprite;
            rightNamecard.text = dialogueSegments[currentIndex].characterName;
        }
        dialogueText.text = dialogueSegments[currentIndex].dialogueLine;

        // Move to next segment
        currentIndex++;
    }

    public void EnterCombat(string sceneName)
    {
        BootstrapSceneManager.Instance.LoadNewScene(SceneManager.GetActiveScene().name, combatSceneName);
    }
}
