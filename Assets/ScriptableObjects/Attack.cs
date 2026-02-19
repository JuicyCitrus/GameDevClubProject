using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]

public class Attack : ScriptableObject
{
    public float baseDamage;
    public float critRate;
    public int priority;
}
