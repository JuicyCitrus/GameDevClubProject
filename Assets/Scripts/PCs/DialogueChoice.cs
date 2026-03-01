using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choice1;
    public DialogueEvent conversationContinuation1;
    public string choice2;
    public DialogueEvent conversationContinuation2;
    public string choice3;
    public DialogueEvent conversationContinuation3;
}
