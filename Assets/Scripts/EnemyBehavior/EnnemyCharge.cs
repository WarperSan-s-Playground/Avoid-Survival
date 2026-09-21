using UnityEngine;

public class EnnemyCharge : MonoBehaviour
{
    [Header("Points Section")]
    [SerializeField] private Transform target;
    public Transform hitPoint;
    private Vector3 objectifPoint;
    [SerializeField] private Transform chargeHitPoint;
    [SerializeField] private Vector2 chargeHitPointSize;

    [Header("States Section")]
    [SerializeField] private bool readyToCharge;
    [SerializeField] private bool charging = false;
    private bool stunned = false;

    [Header("Mob Stats")]
    private float speed;
    [SerializeField] private float chargeDamage;
    [SerializeField] private float remainingChargeDamage;
    [SerializeField] private float chanceToCharge;
    public float stunnedCountDown;
    [SerializeField] private Sprite[] spriteList;

    void Start()
    {
        // Set target
        target = gameObject.GetComponent<EnnemyMove>().target;

        speed = transform.GetComponent<EnnemyMove>().speed * 2f;
        speed *= DifficultyManagerScript.hardcoreSpeedModifier;

        // If hardcore is active
        if (DifficultyManagerScript.hardcoreModeActivated)
        {
            chargeDamage = Random.Range(100f, 400f);
        }
    }

    void Update()
    {
        if (target == null)
        {
            target = gameObject.GetComponent<EnnemyMove>().target;
        }

        // If  the ennemy is charging towards the target
        if (charging)
        {
            Collider2D[] targets = Physics2D.OverlapBoxAll(chargeHitPoint.position, chargeHitPointSize, transform.rotation.z);
            Collider2D hitTarget = null;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].gameObject != gameObject && targets[i].isTrigger == false)
                {
                    hitTarget = targets[i];

                    // Damage the target
                    switch (hitTarget.gameObject.tag)
                    {
                        case "ProtectTarget":
                        case "Ressources":
                            //Damage Ressources
                            MaterialScript script = hitTarget.gameObject.GetComponent<MaterialScript>();
                            remainingChargeDamage -= script.MaterialHit(remainingChargeDamage);

                            if (DifficultyManagerScript.protectTargetActivated)
                            {
                                StoppedCharging();
                            }

                            break;
                        case "Block":
                            //Damage Block
                            remainingChargeDamage -= hitTarget.gameObject.GetComponent<Block>().BlockDamage(remainingChargeDamage);
                            break;
                        case "Player":
                            // Damage the player
                            remainingChargeDamage -= hitTarget.gameObject.GetComponent<MovingCharacter>().PlayerDamage(Mathf.FloorToInt(remainingChargeDamage));
                            StoppedCharging();
                            return;
                        case "Ennemy":
                        case "OwnEnemy":
                            // Damage the enemies
                            remainingChargeDamage -= hitTarget.gameObject.GetComponent<EnnemyMove>().MobDamage(Mathf.FloorToInt(remainingChargeDamage));
                            break;
                        case "Wall":
                            remainingChargeDamage = -1f;
                            break;
                        default:
                            break;
                    }

                    // If the charge has no more power
                    if (remainingChargeDamage <= 0)
                    {
                        transform.GetComponent<EnnemyMove>().ableToRotate = false;

                        charging = false;
                        stunned = true;
                        stunnedCountDown = Random.Range(5f, 10f);

                        // Change Sprite
                        gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[0];
                        return;
                    }
                }
            }

            if (!stunned)
            {
                // Sprint towards the target
                transform.position = Vector2.MoveTowards(transform.position, objectifPoint, speed * Time.deltaTime);
                transform.GetComponent<EnnemyMove>().ableToRotate = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[1];

                // If in hardcore
                if (DifficultyManagerScript.hardcoreModeActivated)
                {
                    // Constently move fowards
                    objectifPoint = hitPoint.position;
                    return;
                }

                if (Vector2.Distance(transform.position, objectifPoint) < 0.1f)
                {
                    StoppedCharging();
                }
                else
                {
                    return;
                }
            }
        }

        if (stunned)
        {
            stunnedCountDown -= Time.deltaTime;

            if (stunnedCountDown <= 0)
            {
                stunned = false;
                transform.GetComponent<EnnemyMove>().ableToMove = true;
                transform.GetComponent<EnnemyMove>().ableToRotate = true;

            }
            else
                return;
        }

        // See if there is nothing between the player and the bull
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position);

        hitPoint.position = transform.position;
        readyToCharge = false;

        if (hit.collider != null)
        {
            // If the raycast hit the player, the shield or one of the player's enemy
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Shield") || hit.collider.CompareTag("OwnEnemy"))
            {
                hitPoint.position = hit.collider.transform.position;
                readyToCharge = true;
            }
        }

        if (!readyToCharge)
            return;

        if (Random.value <= chanceToCharge / 100)
        {
            transform.GetComponent<EnnemyMove>().ableToMove = false;
            objectifPoint = hitPoint.position;
            charging = true;
            remainingChargeDamage = chargeDamage;
        }
    }

    private void StoppedCharging()
    {
        charging = false;
        stunned = true;
        stunnedCountDown = Random.Range(5f, 10f);
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[0];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(chargeHitPoint.position, chargeHitPointSize);
    }
}
