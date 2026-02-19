using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLineup", menuName = "Scriptable Objects/EnemyLineup")]

public class EnemyLineup : ScriptableObject
{
    public GameObject[] enemies;
}
