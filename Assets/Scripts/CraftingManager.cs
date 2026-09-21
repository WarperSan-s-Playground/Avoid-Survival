using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    private MovingCharacter player;

    public GameObject invContentParent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovingCharacter>();

        if (player == null)
        {
            Debug.Log("No Player Detected");

            Destroy(gameObject);
            return;
        }
    }

    public void craftItem(CraftingRecipe button)
    {
        // Take the needed materials and the quantites
        string[] needMaterials = button.materials;
        int[] quantity = button.quantity;

        // If the inventory of the player is empty
        if (player.inventory.Count == 0)
            return;

        // Verify if every item needed are available
        for (int i = 0; i < needMaterials.Length; i++)
        {
            if (!player.inventory.Contains(needMaterials[i]))
            {
                Debug.Log("Missing material " + needMaterials[i]);
                return;
            }
            else
            {
                int index = player.inventory.IndexOf(needMaterials[i]);

                if (player.amount[index] < quantity[i])
                {
                    Debug.Log("Missing quantity");
                    return;
                }
            }
        }

        for (int i = 0; i < needMaterials.Length; i++)
        {
            int index = player.inventory.IndexOf(needMaterials[i]);

            player.UpdateSlot(index, -1 * quantity[i], needMaterials[i]);
        }

        // If the player already has at least one copy in his inventory
        if (player.inventory.Contains(button.nameOfItem))
        {
            player.UpdateSlot(player.inventory.IndexOf(button.nameOfItem), button.quantityGiven, button.nameOfItem);
            //Debug.Log(player.inventory.IndexOf(button.nameOfItem));
        }
        else
        {
            // Create a slot for the item
            player.CreateSlot(button.quantityGiven, button.nameOfItem, button.itemSlotColor);
            ItemSlot itemSlot = invContentParent.transform.Find(button.nameOfItem).GetComponent<ItemSlot>();

            // If the item has a prefab (blocks, traps...)
            if (button.blockPrefab != null)
                itemSlot.blockToPlace = button.blockPrefab;

            itemSlot.typeOfItem = button.typeOfItem;
        }
    }
}
