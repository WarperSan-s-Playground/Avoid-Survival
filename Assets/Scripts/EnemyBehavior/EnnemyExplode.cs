using UnityEngine;

public class EnnemyExplode : MonoBehaviour
{
    [SerializeField] private float explosingCountdown;
    [SerializeField] private float currentCountDown;
    [SerializeField] private float minDistanceToExplode = 1f;
    [SerializeField] private int explosionDamage = 10;
    private bool exploded = false;
    private EnnemyMove ennemyMove;
    [SerializeField] private Sprite[] spriteList;

    // Start is called before the first frame update
    void Start()
    {
        ennemyMove = GetComponent<EnnemyMove>();
        currentCountDown = explosingCountdown;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded)
            return;

        if (Vector2.Distance(transform.position, ennemyMove.target.position) <= minDistanceToExplode)
        {
            currentCountDown -= Time.deltaTime;
            transform.GetComponent<SpriteRenderer>().sprite = spriteList[1];

            if (currentCountDown <= 0)
            {
                Debug.Log("Exploded");
                exploded = true;

                Explosion();
            }
        }
        else
        {
            currentCountDown = explosingCountdown;
            transform.GetComponent<SpriteRenderer>().sprite = spriteList[0];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, minDistanceToExplode);
    }

    private void Explosion()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, minDistanceToExplode);

        foreach (Collider2D target in targets)
        {
            switch (target.gameObject.tag)
            {
                case "ProtectTarget":
                case "Ressources":
                    //Damage Ressources
                    MaterialScript script = target.gameObject.GetComponent<MaterialScript>();
                    script.MaterialHit(explosionDamage);
                    break;
                case "Block":
                    //Damage Block
                    target.gameObject.GetComponent<Block>().BlockDamage(explosionDamage);
                    break;
                case "Player":
                    // Damage the player
                    target.gameObject.GetComponent<MovingCharacter>().PlayerDamage(explosionDamage);
                    break;
                case "Ennemy":
                    // Damage the ennemies
                    target.gameObject.GetComponent<EnnemyMove>().MobDamage(explosionDamage);
                    break;
                default:
                    break;
            }
        }

        Destroy(gameObject);
    }
}
