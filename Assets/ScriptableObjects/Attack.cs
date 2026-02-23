using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]

public class Attack : ScriptableObject
{
    public int baseDamage;
    public float critRate;
    public int priority;
}
