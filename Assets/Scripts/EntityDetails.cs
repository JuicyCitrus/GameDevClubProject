using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class EntityDetails : MonoBehaviour
{
    [Header("Entity Details")]
    public string entityName;
    public GameObject combatPrefab;
    public SpriteRenderer entitySprite;
    public bool isDead = false;

    [Header("Entity Stats")]
    public int baseAttack;
    public int speed;

    [Header("Entity Moves")]
    public Attack[] attacks;

    [Header("Health Components")]
    public int maxHealth;
    public int currentHealth;
    public Image healthbar;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void AdjustHealth(int amount)
    {
        currentHealth += amount;
        healthbar.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        entitySprite.color = Color.black;
        isDead = true;
    }
}
