using UnityEngine;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour
{
    public float health;
    public Slider shieldSlider;

    private void Start()
    {
        health = DifficultyManagerScript.defaultShieldHealth;

        shieldSlider.maxValue = health;
    }

    public float damageShield(float amount)
    {
        float healthTemp = health;

        health -= amount;

        shieldSlider.value = health;

        // If the damage kills the shield
        if (health <= 0)
        {
            gameObject.SetActive(false);

            // Return the remaining damages
            return amount - healthTemp;
        }

        return 0;
    }

    public void shieldUpdate()
    {
        shieldSlider.maxValue = DifficultyManagerScript.defaultShieldHealth;
        shieldSlider.gameObject.SetActive(true);
    }
}
