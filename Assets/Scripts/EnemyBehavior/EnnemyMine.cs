using UnityEngine;

public class EnnemyMine : MonoBehaviour
{
    private Vector2 digDestination;
    [SerializeField] private float currentCountDown;
    [SerializeField] private float initialCountDown = 10f;
    [SerializeField] private bool digging = false;
    [SerializeField] private float diggingSpeed = 1f;
    [SerializeField] private float attackRadius;
    [SerializeField] private int Damage;
    [SerializeField] private Sprite[] spriteList;

    private void Start()
    {
        currentCountDown = initialCountDown;
    }

    private void Update()
    {
        // If the ennemy is digging
        if (digging == true)
        {
            // Dig
            Digging();
            return;
        }

        // If the countdown is not finished
        if (currentCountDown > 0)
        {
            // Reduce the current countdown
            currentCountDown -= Time.deltaTime;
            return;
        }

        currentCountDown = initialCountDown;

        // Search for a valid point
        int randomIndex = Random.Range(0, MapManager.instance.spawningAreas.Length);

        Transform[,] points = MapManager.instance.points;
        float randX = Random.Range(points[randomIndex,0].position.x, points[randomIndex, 1].position.x);
        float randY = Random.Range(points[randomIndex, 0].position.y, points[randomIndex, 1].position.y);

        digDestination = new Vector3(randX, randY, 1f);

        // Desactivate the ennemy's collider
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        digging = true;
    }

    private void Digging()
    {
        Vector2 digPos = Vector2.MoveTowards(transform.position, digDestination, diggingSpeed * Time.deltaTime);
        transform.position = new Vector3(digPos.x, digPos.y, 1f);
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[1];

        if (Vector2.Distance(transform.position, digDestination) <= 0.2f)
        {
            digging = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[0];

            if (Random.value <= 0.5f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            }

            transform.position = new Vector3(digPos.x, digPos.y, 0f);
            gameObject.GetComponent<CircleCollider2D>().enabled = true;

            Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, attackRadius);

            if (hitTargets.Length == 1 && hitTargets[0].gameObject == gameObject)
                return;

            foreach (var target in hitTargets)
            {
                switch (target.gameObject.tag)
                {
                    case "ProtectTarget":
                    case "Ressources":
                        //Damage Ressources
                        MaterialScript script = target.gameObject.GetComponent<MaterialScript>();
                        script.MaterialHit(Damage);
                        break;
                    case "Block":
                        //Damage Block
                        target.gameObject.GetComponent<Block>().BlockDamage(Damage);
                        break;
                    case "Player":
                        target.gameObject.GetComponent<MovingCharacter>().PlayerDamage(Damage);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
