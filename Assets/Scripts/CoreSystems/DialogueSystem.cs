using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [Header("Dialogue Settings")]
    public DialogueEvent dialogue;

    [Header("Combat Settings")]
    public bool enterCombatAfterDialogue = false;
    public string combatSceneName;
    public EnemyLineup combatEnemies;

    [Header("UI Elements")]
    public Canvas dialogueCanvas;
    public Image leftSideSprite;
    public Image rightSideSprite;
    public Color inactiveSpriteTone;
    public GameObject leftNamecard;
    public GameObject rightNamecard;
    public TextMeshProUGUI leftNamecardText;
    public TextMeshProUGUI rightNamecardText;
    public TextMeshProUGUI dialogueText;

    [Header("Choice Buttons")]
    public Button_DialogueChoice choiceButton1;
    public Button_DialogueChoice choiceButton2;
    public Button_DialogueChoice choiceButton3;
    public TextMeshProUGUI choiceText1;
    public TextMeshProUGUI choiceText2;
    public TextMeshProUGUI choiceText3;

    private Controls controls;
    public int currentIndex = 0;
    public bool choicesAreActive = false;
    public bool displayChoicesNext = false;
    private bool textIsRevealing = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        controls = new Controls();
    }

    public void ActivateDialogue()
    {
        // Reset dialogue index so it doesn't immediately end when Advance Dialogue is called
        currentIndex = 0;

        // Reset choice buttons
        DeactivateChoiceButtons();

        // Stop time
        Time.timeScale = 0f;

        // Enable dialogue UI and input
        controls.UI.Enable();
        controls.UI.Submit.performed += ctx => AdvanceDialogue();

        // Set the combat bool to true if the event has an enemy lineup
        enterCombatAfterDialogue = dialogue.enemyLineup != null;
        combatEnemies = dialogue.enemyLineup;

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
        // Do not advance dialogue if choice buttons are active
        if (choicesAreActive)
            return;

        // End dialogue if no more segments
        if (currentIndex >= dialogue.segments.Count)
        {
            DeactivateDialogue();

            // Enter the combat scene if this dialogue is meant to lead into combat
            if (enterCombatAfterDialogue)
            {
                // Fill the Combatants static class with the enemies for the next combat scene, then load it
                Combatants.currentLineup = combatEnemies;

                // Load the scene
                EnterCombat(combatSceneName);
            }

            return;
        }

        // If the next segment has a choice, activate the choice buttons and return before anything else gets activated
        if (displayChoicesNext)
        {
            // Finish displaying the line if the text is still being revealed so the player can see what they're responding to
            if(textIsRevealing)
            {
                StopAllCoroutines();
                dialogueText.text = dialogue.segments[currentIndex].dialogueLine;
                textIsRevealing = false;
            }

            // Activate the choice buttons
            ActivateChoiceButtons(dialogue.segments[currentIndex].choice);
            displayChoicesNext = false;
            return;
        }

        // Finish displaying the line if the text is still being revealed rather than advancing to the next line
        if (textIsRevealing)
        {
            StopAllCoroutines();
            dialogueText.text = dialogue.segments[currentIndex].dialogueLine;
            textIsRevealing = false;
            currentIndex++;
            return;
        }

        // Update dialogue UI
        if (dialogue.segments[currentIndex].isLeftSide)
        {
            // Activate left side of dialogue canvas
            leftSideSprite.sprite = dialogue.segments[currentIndex].characterImage.sprite;
            leftSideSprite.color = Color.white;
            leftNamecard.SetActive(true);
            leftNamecardText.text = dialogue.segments[currentIndex].characterName;

            // Deactivate right side of dialogue canvas
            rightNamecard.SetActive(false);
            rightSideSprite.color = inactiveSpriteTone;
        }
        else
        {
            // Activate right side of dialogue canvas
            rightSideSprite.sprite = dialogue.segments[currentIndex].characterImage.sprite;
            rightSideSprite.color = Color.white;
            rightNamecard.SetActive(true);
            rightNamecardText.text = dialogue.segments[currentIndex].characterName;

            // Deactivate left side of dialogue canvas
            leftNamecard.SetActive(false);
            leftSideSprite.color = inactiveSpriteTone;
        }

        // Set the dialogue text after resetting it to nothing
        dialogueText.text = "";
        StopAllCoroutines();
        StartCoroutine(RevealText());

        // If a choice needs to be made, set the bool and do not index up, otherwise, index up as normal
        if (dialogue.segments[currentIndex].choice.choice1 != "")
        {
            displayChoicesNext = true;
        }
        // If text is being revealed, do not index up. The string will fill completely on the next click rather than going to the next line.
        else if (textIsRevealing)
        {
            return;
        }
        else
        {
            currentIndex++;
        }
    }

    public void EnterCombat(string sceneName)
    {
        BootstrapSceneManager.Instance.LoadNewScene(SceneManager.GetActiveScene().name, combatSceneName);
    }

    private void ActivateChoiceButtons(DialogueChoice choice)
    {
        if(choice.choice1 != "")
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton1.nextPartOfConversation = choice.conversationContinuation1;
            choiceText1.text = choice.choice1;
        }

        if (choice.choice2 != "")
        {
            choiceButton2.gameObject.SetActive(true);
            choiceButton2.nextPartOfConversation = choice.conversationContinuation2;
            choiceText2.text = choice.choice2;
        }

        if (choice.choice3 != "")
        {
            choiceButton3.gameObject.SetActive(true);
            choiceButton3.nextPartOfConversation = choice.conversationContinuation3;
            choiceText3.text = choice.choice3;
        }

        choicesAreActive = true;
    }

    private void DeactivateChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
        choicesAreActive = false;
        displayChoicesNext = false;
    }

    private IEnumerator RevealText()
    {
        textIsRevealing = true;

        for (int currentLetter = 0; currentLetter <= dialogue.segments[currentIndex].dialogueLine.Length - 1; currentLetter++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            dialogueText.text += dialogue.segments[currentIndex].dialogueLine[currentLetter];
        }

        // Only index up if there is not a choice to be made next
        if (!displayChoicesNext)
            currentIndex++;

        textIsRevealing = false;
    }
}
