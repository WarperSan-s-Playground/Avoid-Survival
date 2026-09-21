using UnityEngine;

public class summoningStaffScript : MonoBehaviour
{
    public int tier = -1;
    public float energyConsummation = 1f;
    public int durability;
    public string itemName = "";
    [SerializeField] private int[] durabilityArray;

    public GameObject[] monstersArray;
    [SerializeField] private Sprite[] spriteArray;

    public void ResetStaff()
    {
        if (monstersArray.Length - 1 < tier)
        {
            tier = monstersArray.Length - 1;
        }

        GetComponent<SpriteRenderer>().sprite = spriteArray[tier];

        durability = durabilityArray[tier];
    }

    public void SpawnEnemy(MovingCharacter player)
    {
        if (durability <= 0)
            return;

        if (!DifficultyManagerScript.infiniteSprinting && player.energyBar.value - energyConsummation - tier < 0)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject monsterToSpawn;

        monsterToSpawn = monstersArray[tier];


        Instantiate(monsterToSpawn, new Vector3(mousePos.x, mousePos.y, 1f), Quaternion.identity);

        durability--;

        if (!DifficultyManagerScript.infiniteSprinting)
            player.energyBar.value -= energyConsummation + tier;

        if (durability <= 0)
        {
            player.UpdateSlot(player.inventory.IndexOf(itemName), -1, itemName);
            gameObject.SetActive(false);
            itemName = "";
            tier = -1;
        }
    }
}
