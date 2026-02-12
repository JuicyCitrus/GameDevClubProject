using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3[] spawnPoints;

    private void Start()
    {
        int size = Combatants.enemyPrefabs.Length;

        for(int i = 0; i < size; i++)
        {
            Instantiate(Combatants.enemyPrefabs[i], spawnPoints[i], Quaternion.identity);
        }
    }
}
