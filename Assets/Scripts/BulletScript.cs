using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector2 originPos;
    public int damage;
    public string parentTag;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, originPos) > 10)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == parentTag)
            return;

        if (collision.tag == gameObject.tag)
            return;

        //If the bullet touch something else then an ennemy
        switch (collision.tag)
        {
            //If the collision is the player or the shield
            case "Shield":
            case "Player":
                GameObject.FindGameObjectWithTag("Player").GetComponent<MovingCharacter>().PlayerDamage(damage);
                //Damage Player
                break;
            //If the collision is a block
            case "Block":
                collision.gameObject.GetComponent<Block>().BlockDamage(damage);
                break;
            case "ProtectTarget":
            case "Ressources":
                collision.gameObject.GetComponent<MaterialScript>().MaterialHit(damage);
                break;
            case "Ennemy":
                if (parentTag == collision.tag)
                    return;

                collision.GetComponent<EnnemyMove>().MobDamage(damage);
                break;
            case "OwnEnemy":
                if (parentTag == collision.tag)
                    return;

                collision.GetComponent<EnnemyMove>().MobDamage(damage);
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
