using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueEvent", menuName = "Scriptable Objects/DialogueEvent")]

public class DialogueEvent : ScriptableObject
{
    public EnemyLineup enemyLineup;
    public List<DialogueSegment> segments = new List<DialogueSegment>();
}