using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int usage = 3;
    [SerializeField] private OtherHealthBarManager healthBar;

    private void Start()
    {
        healthBar.healthBar.maxValue = usage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ennemy":
                collision.gameObject.GetComponent<EnnemyMove>().MobDamage(damage);
                break;
            case "Player":
                collision.gameObject.GetComponent<MovingCharacter>().PlayerDamage(damage);
                break;
            default:
                return;
        }

        usage--;
        healthBar.UpdateHealthBar(usage);

        if (usage == 0)
        {
            Destroy(gameObject);
        }
    }
}
