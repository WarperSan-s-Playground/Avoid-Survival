using UnityEngine;

public class EnnemyPunch : MonoBehaviour
{
    [SerializeField] private Transform punchOrigin;
    [SerializeField] private Vector2 punchSize;
    [SerializeField] private float currentCountDown;
    [SerializeField] private EnnemyMove enemy;

    [Header("Stats")]
    [SerializeField] private bool randomValues;
    [SerializeField] private float initialCountDown;
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        if (randomValues == true)
        {
            damage = Mathf.FloorToInt(Random.Range(1f, 5f));
            initialCountDown = Random.Range(2f, 6f);
        }

        currentCountDown = initialCountDown;

        enemy = transform.GetComponent<EnnemyMove>();
    }

    private void Update()
    {
        if (enemy.target == null)
            return;

        if (currentCountDown <= 0)
        {
            currentCountDown = initialCountDown;
            Punch();
        }
        else
        {
            currentCountDown -= Time.deltaTime;
        }
    }

    private void Punch()
    {
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(punchOrigin.position, punchSize, transform.rotation.z);

        if (hitTargets.Length == 0)
            return;

        foreach (var target in hitTargets)
        {
            switch (target.gameObject.tag)
            {
                case "ProtectTarget":
                case "Ressources":
                    //Damage Ressources
                    MaterialScript script = target.gameObject.GetComponent<MaterialScript>();
                    script.MaterialHit(damage);
                    break;
                case "Block":
                    //Damage Block
                    target.gameObject.GetComponent<Block>().BlockDamage(damage);
                    break;
                case "Shield":
                    GameObject.FindGameObjectWithTag("Shield").GetComponent<ShieldScript>().damageShield(damage);
                    return;
                case "Player":
                    MovingCharacter player = target.gameObject.GetComponent<MovingCharacter>();

                    if (player.shield.gameObject.activeInHierarchy)
                    {
                        GameObject.FindGameObjectWithTag("Shield").GetComponent<ShieldScript>().damageShield(damage);
                        return;
                    }

                    player.PlayerDamage(damage);
                    return;
                case "Ennemy":
                    if (transform.GetComponent<EnnemyMove>().playerMod)
                    {
                        target.GetComponent<EnnemyMove>().MobDamage(damage);
                    }
                    break;
                case "OwnEnemy":
                    if (!transform.GetComponent<EnnemyMove>().playerMod)
                    {
                        target.GetComponent<EnnemyMove>().MobDamage(damage);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(punchOrigin.position, punchSize);
    }
}
