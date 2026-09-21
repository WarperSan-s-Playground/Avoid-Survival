using UnityEngine;

public class EnnemyShoot : MonoBehaviour
{

    public Transform target; //set target from inspector instead of looking in Update
    private EnnemyMove enemy;

    public GameObject bullet;
    private float shootCountdown;
    public float countDown;
    [SerializeField] private Sprite[] spriteList;

    private void Start()
    {
        shootCountdown = countDown;

        enemy = GetComponent<EnnemyMove>();
        target = enemy.target;
    }

    void Update()
    {
        if (enemy.playerMod)
        {
            target = enemy.target;
        }

        ShootAtTarget();
    }

    private void ShootAtTarget()
    {
        if (target == null)
            return;

        if (shootCountdown > 0)
        {
            if (shootCountdown < countDown / 3)
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[1];

            shootCountdown -= Time.deltaTime;
        }
        else
        {
            shootCountdown = countDown;
            GameObject bulletInst = Instantiate(bullet, transform.position, Quaternion.identity);

            bulletInst.GetComponent<BulletScript>().originPos = transform.position;
            bulletInst.GetComponent<Rigidbody2D>().linearVelocity = transform.right * 3;
            bulletInst.GetComponent<BulletScript>().parentTag = transform.tag;

            gameObject.GetComponent<SpriteRenderer>().sprite = spriteList[0];
        }
    }
}