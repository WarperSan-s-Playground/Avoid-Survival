using UnityEngine;

public class MaterialScript : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] private OtherHealthBarManager healthBar;
    public string materialName;
    public float chanceOfDrop = 0.5f; // Default 50% chance of drop

    public bool targetToProtected;

    private void Start()
    {
        healthBar.healthBar.maxValue = health;
        healthBar.healthBar.value = health;

        DifficultyManagerScript.materialsChanceDrop.TryGetValue(materialName, out float value);
        chanceOfDrop = value;
    }

    public float MaterialHit(float amount)
    {
        float healthTemp = health;

        // Remove the amount of the health and update the health bar
        health -= amount;
        healthBar.UpdateHealthBar(Mathf.FloorToInt(health));

        // If there is no HP left, destroy the gameobject
        if (health <= 0)
        {
            if (targetToProtected == true)
            {
                MovingCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovingCharacter>();

                player.gameOverAnimator.Play("GameOverMenu");
                player.gameEnded = true;
            }


            Destroy(gameObject);
            return healthTemp;
        }

        return amount;
    }
}
