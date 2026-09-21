using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float health = 20f;
    [SerializeField] private OtherHealthBarManager healthBar;

    private void Start()
    {
        healthBar.healthBar.maxValue = health;
    }

    public float BlockDamage(float amount)
    {
        float healthTemp = health;

        // Remove the amount of the health and update the health bar
        health -= amount;
        healthBar.UpdateHealthBar(Mathf.FloorToInt(health));

        // If there is no HP left, destroy the gameobject
        if (health <= 0)
        {
            Destroy(gameObject);
            return healthTemp;
        }

        return amount;
    }
}
