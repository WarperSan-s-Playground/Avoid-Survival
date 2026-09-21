using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingCharacter : MonoBehaviour
{
    [Header("Player Stats")]
    public float currentSpeed;
    public bool gameEnded = false;

    [SerializeField] private float runningSpeed;
    [SerializeField] private float normalSpeed;
    public float speedModifier = 1;
    [SerializeField] private int health;
    [SerializeField] private float energy;
    public float Damage;

    public GameObject shield;

    [Header("Weapons")]
    public GameObject[] weaponList;
    [Space]
    public GameObject summoningStaff;
    public GameObject clubWeapon;
    public GameObject daggerWeapon;
    public GameObject bowWeapon;
    public GameObject arrowPrefab;
    public GameObject gunWeapon;
    public GameObject bulletPrefab;

    [Header("UI")]
    public Slider healthBar;
    public Slider energyBar;
    public Animator gameOverAnimator;
    public GameObject crafting;

    [Header("Inventory")]
    public List<string> inventory;
    public List<int> amount;
    public GameObject slotPrefab;
    public GameObject slotContainer;

    [Header("Cutting Tree")]
    public Transform cuttingTreeOrigin;
    public Vector2 cuttingTreeRange;

    [Header("Building")]
    public GameObject blockToPlace = null;
    public GameObject blockDisplay;

    [Header("Miscellaneous")]
    private Rigidbody2D rb;
    [SerializeField] private Sprite specialPlayerSprite;

    private void Start()
    {
        if (Random.value <= 0.1f)
        {
            GetComponent<SpriteRenderer>().sprite = specialPlayerSprite;
        }

        rb = GetComponent<Rigidbody2D>();

        health = (int)DifficultyManagerScript.playerBaseHealth;

        healthBar.maxValue = health;
        healthBar.value = health;

        energyBar.maxValue = energy;
        energyBar.value = energy;

        // Set the speeds
        normalSpeed = DifficultyManagerScript.playerWalkingSpeed;
        runningSpeed = DifficultyManagerScript.playerRunningSpeed;

        // Activate or not the shield at the start of the game
        shield.SetActive(DifficultyManagerScript.startWithShield);

        if (shield.activeInHierarchy)
        {
            shield.GetComponent<ShieldScript>().shieldUpdate();
        }

        // Staff
        summoningStaff = transform.Find("SummoningStaff").gameObject;
    }

    private void Update()
    {
        if (gameEnded)
            return;

        // If no block has been choose to be placed
        if (blockToPlace != null)
        {
            BuildingManager.instance.BuildingCheck(this);
        }
        else
        {
            blockDisplay.GetComponent<SpriteRenderer>().sprite = null;
        }

        // If the player presses E
        // Open the crafting UI
        if (Input.GetKeyDown(KeyCode.E))
        {
            crafting.gameObject.SetActive(!crafting.activeInHierarchy);
            blockToPlace = null;

            TooltipSystem.Hide();
            return;
        }

        // If the crafting UI is open, stop
        if (crafting.activeInHierarchy)
            return;

        FaceMouse();

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && energyBar.value > 0)
        {
            currentSpeed = runningSpeed * speedModifier;
        }
        else
        {
            currentSpeed = normalSpeed * speedModifier;
        }

        // If the player left clicks
        if (Input.GetMouseButtonDown(0))
        {
            // Exit when the player clicks on the UI
            if (IsMouseOverUI())
                return;

            // If the bow is active
            if (bowWeapon.activeInHierarchy)
            {
                // If the player has no arrow
                if (!inventory.Contains("Arrow"))
                    return;

                if (!UpdateSlot(inventory.IndexOf("Arrow"), -1, "Arrow"))
                    return;

                GameObject bulletInst = Instantiate(arrowPrefab, transform.position, transform.rotation);

                bulletInst.GetComponent<BulletScript>().originPos = transform.position;
                bulletInst.GetComponent<BulletScript>().damage = Mathf.FloorToInt(bowWeapon.GetComponent<ShootWeaponClass>().bulletDamage);
                bulletInst.GetComponent<Rigidbody2D>().linearVelocity = transform.right * bowWeapon.GetComponent<ShootWeaponClass>().bulletSpeed;
                bulletInst.GetComponent<BulletScript>().parentTag = transform.tag;
                return;
            }

            // If the gun is active
            if (gunWeapon.activeInHierarchy)
            {
                // If the player has no arrow
                if (!inventory.Contains("Bullet"))
                    return;

                if (!UpdateSlot(inventory.IndexOf("Bullet"), -1, "Bullet"))
                    return;

                GameObject bulletInst = Instantiate(arrowPrefab, transform.position, transform.rotation);

                bulletInst.GetComponent<BulletScript>().originPos = transform.position;
                bulletInst.GetComponent<BulletScript>().damage = Mathf.FloorToInt(bowWeapon.GetComponent<ShootWeaponClass>().bulletDamage);
                bulletInst.GetComponent<Rigidbody2D>().linearVelocity = transform.right * bowWeapon.GetComponent<ShootWeaponClass>().bulletSpeed;
                bulletInst.GetComponent<BulletScript>().parentTag = transform.tag;
                return;
            }

            // When every condition is incorrect, make the player punch
            PlayerPunch();
        }

        if (inventory.Count > 0 && Input.GetMouseButtonDown(1) && blockToPlace != null)
        {
            BuildingManager.instance.Building(this);
            return;
        }

        if (summoningStaff.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(1))
            {
                summoningStaff.GetComponent<summoningStaffScript>().SpawnEnemy(this);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(cuttingTreeOrigin.position, cuttingTreeRange);
        Gizmos.DrawWireSphere(transform.position, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameEnded)
            return;

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * currentSpeed * Time.deltaTime;

        // If the player is running while not being in the crafting UI
        if (currentSpeed == runningSpeed && moveInput != Vector2.zero && !crafting.activeInHierarchy)
        {
            // If the player has not infinite sprinting on
            if (!DifficultyManagerScript.infiniteSprinting)
            {
                energyBar.value -= Time.deltaTime * DifficultyManagerScript.energyLostRate;

                if (DifficultyManagerScript.staminaModeActivated == true)
                {
                    if (energyBar.value <= 0)
                    {
                        PlayerDamage(health);
                    }
                }
            }
        }
        // If the player is not moving
        else if (moveInput == Vector2.zero)
        {
            // Restore energy over time
            if (energyBar.value + Time.deltaTime * DifficultyManagerScript.energyGainRate > energyBar.maxValue)
            {
                energyBar.value = energyBar.maxValue;
            }
            else
            {
                energyBar.value += Time.deltaTime * DifficultyManagerScript.energyGainRate;
            }
        }

        if (crafting.activeInHierarchy)
            return;

        rb.MovePosition(rb.position + moveInput);
    }

    private void FaceMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    public void CreateSlot(int count, string name, Color itemSlotColor)
    {
        //Add the item in the lists
        inventory.Add(name);
        amount.Add(count);

        // Add the item slot
        GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);

        // Set it's color
        newSlot.transform.Find("Image").GetComponent<Image>().color = itemSlotColor;

        //Create a new icon
        newSlot.name = name;
        newSlot.GetComponent<ItemSlot>().itemName = name;

        newSlot.transform.GetChild(0).Find("Title").GetComponent<Text>().text = name;
        newSlot.transform.GetChild(0).Find("Amount").GetComponent<Text>().text = count.ToString();
    }

    public bool UpdateSlot(int index, int count, string name)
    {
        //If the player has enough item
        if (amount[index] + count >= 0)
        {
            // Add/Remove the amount of item
            amount[index] += count;

            // Get the slot corresponding to the item
            GameObject slot = slotContainer.transform.Find(name).gameObject;

            // Update the amount
            slot.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = amount[index].ToString();

            // If the slot is empty
            if (amount[index] == 0)
            {
                // Remove the slot
                inventory.RemoveAt(index);
                amount.RemoveAt(index);

                Destroy(slot);
            }

            return true;
        }
        return false;
    }

    public int PlayerDamage(int damageCount)
    {
        // Damage shield if active

        // If the shield is active
        if (shield.activeInHierarchy)
        {
            damageCount = (int)shield.GetComponent<ShieldScript>().damageShield(damageCount);
        }

        float healthTemp = health;

        // Remove the amount of damage
        if (!DifficultyManagerScript.infiniteHealth)
            health -= damageCount;

        // If the health of the player is dead
        if (health <= 0 && gameEnded == false)
        {
            //Game Over
            gameOverAnimator.Play("GameOverMenu");
            health = 0;
            gameEnded = true;

            // Update the healthBar
            healthBar.value = health;

            return (int)healthTemp;
        }

        // Update the healthBar
        healthBar.value = health;

        return damageCount;
    }

    private void PlayerPunch()
    {
        //Search every object in collision of his cutting range
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(cuttingTreeOrigin.position, cuttingTreeRange, transform.rotation.z);

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
                    script.MaterialHit(Damage);

                    if (script.targetToProtected == true)
                        return;

                    string materialName = script.materialName;

                    // Put a chance of drop
                    // Each material has his unique chance of drop

                    if (Random.value > script.chanceOfDrop)
                        return;

                    if (MapManager.instance.survivalGame)
                        return;

                    if (!inventory.Contains(materialName))
                    {
                        CreateSlot(1, materialName, Color.white);
                    }
                    else
                    {
                        UpdateSlot(inventory.IndexOf(materialName), 1, materialName);
                    }

                    return;
                case "Block":
                    //Damage Block
                    target.gameObject.GetComponent<Block>().BlockDamage(Damage);
                    return;
                case "Ennemy":
                case "OwnEnemy":
                    //Damage Ennemy
                    target.gameObject.GetComponent<EnnemyMove>().MobDamage((int)Damage);
                    return;
                default:
                    break;
            }
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
