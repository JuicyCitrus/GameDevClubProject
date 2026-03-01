using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueSegment
{
    public string dialogueLine;
    public Image characterImage;
    public string characterName;
    public bool isLeftSide;
    public DialogueChoice choice;
}
