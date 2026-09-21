using UnityEngine;

public class EnnemyMove : MonoBehaviour
{

    public Transform target;//set target from inspector instead of looking in Update

    [Header("Mob Stats")]
    [SerializeField] private bool randomSpeed = true;
    public bool playerMod = false;
    public float minSpeed = 0.5f;
    public float maxSpeed = 5f;
    public float speed = 3f;
    [Space]
    [SerializeField] private bool randomOffset = true;
    public float minOffset = -7f;
    public float maxOffset = 7f;
    public float offset = 3f;
    [Space]
    [SerializeField] private bool randomDistance = true;
    public float minDistance = 3f;
    public float maxDistance = 7f;
    public float minDistanceSelected = 3f;

    [Space]
    [Space]
    [SerializeField] private float health;
    [SerializeField] private bool keepDistance;
    public OtherHealthBarManager personalHealthBar;
    public bool ableToMove = true;
    public bool ableToRotate = true;

    private void Start()
    {
        if (randomOffset)
        {
            offset = Random.Range(minOffset, maxOffset);
        }

        if (randomSpeed)
        {
            speed = Random.Range(minSpeed, maxSpeed);
        }

        if (randomDistance)
        {
            minDistanceSelected = Random.Range(minDistance, maxDistance);
        }

        if (DifficultyManagerScript.protectTargetActivated == false)
        {
            if (playerMod)
            {
                SearchForTarget();
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;

                if (target == null)
                {
                    Debug.Log("No Player Detected");
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("ProtectTarget").transform;
        }

        if (DifficultyManagerScript.hardcoreModeActivated)
        {
            speed *= DifficultyManagerScript.hardcoreSpeedModifier;
            health *= DifficultyManagerScript.hardcoreHealthModifier;
        }


        personalHealthBar.healthBar.maxValue = health;
    }

    void Update()
    {
        if (playerMod)
        {
            SearchForTarget();
        }

        if (ableToRotate)
            LookTarget();

        if (ableToMove)
            MoveToTarget();
    }

    private void SearchForTarget()
    {
        GameObject targetTemp = GameObject.FindGameObjectWithTag("Ennemy");

        if (targetTemp == null)
        {
            target = null;
        }
        else
            target = targetTemp.transform;
    }

    private void LookTarget()
    {
        if (target == null)
            return;

        Vector3 difference = target.position - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset);
    }

    private void MoveToTarget()
    {
        if (target == null)
            return;

        //// If the enemy wants to stay far from the player
        if (keepDistance == true)
        {
            if (Vector2.Distance(transform.position, target.position) <= minDistanceSelected)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -1 * speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, target.position) >= minDistanceSelected * 1.2f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public float MobDamage(int amount)
    {
        health -= amount;

        personalHealthBar.UpdateHealthBar((int)health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        return health;
    }
}