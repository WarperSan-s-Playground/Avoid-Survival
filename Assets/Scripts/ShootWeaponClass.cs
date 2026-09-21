using UnityEngine;

public class ShootWeaponClass : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;

    private void Start()
    {
        if (bulletSpeed == 0)
        {
            bulletSpeed = 1;
        }
    }
}
