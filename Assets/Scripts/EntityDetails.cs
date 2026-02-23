using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class EntityDetails : MonoBehaviour
{
    [Header("Entity Details")]
    public string entityName;
    public GameObject combatPrefab;

    [Header("Entity Visuals")]
    public SpriteRenderer entitySprite;
    public Color entityColor;

    [Header("Entity Stats")]
    public int baseAttack;
    public int speed;

    [Header("Entity Moves")]
    public Attack[] attacks;

    [Header("Health Components")]
    public int maxHealth;
    public int currentHealth;
    public Image healthbar;
    public bool isDead = false;

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        Reset();
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

    private void Reset()
    {
        entitySprite.color = entityColor;
        currentHealth = maxHealth;
        isDead = false;
        healthbar.fillAmount = 1;
    }
}
