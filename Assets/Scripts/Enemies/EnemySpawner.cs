using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector3[] spawnPoints;

    private void Start()
    {
        int index = 0;

        foreach(GameObject enemy in Combatants.currentLineup.enemies)
        {
            Vector3 spawnPoint = spawnPoints[index];
            Instantiate(enemy, spawnPoint, Quaternion.identity);
            index++;
        }
    }
}
