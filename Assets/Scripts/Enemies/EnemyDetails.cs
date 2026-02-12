using UnityEngine;
using UnityEngine.UI;

public class EnemyDetails : MonoBehaviour
{
    public string enemyName;

    [Header("Health Components")]
    public int maxHealth;
    public int currentHealth;
    public Image healthbar;

    public int attackDamage;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }
}
