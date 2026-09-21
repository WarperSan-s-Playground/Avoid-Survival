using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MovingCharacter player;

    public string header;
    [Multiline()]
    public string content;

    private void Start()
    {
        if (TooltipSystem.current.inMenu)
            return;

        GameObject player1 = GameObject.FindGameObjectWithTag("Player");

        if (player1 == null)
        {
            Debug.Log("No Player Detected");

            Destroy(gameObject);
            return;
        }

        player = player1.GetComponent<MovingCharacter>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<CraftingRecipe>() != null)
        {
            UpdateText();
            return;
        }

        TooltipSystem.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    public void UpdateText()
    {
        // Empty the content
        content = "";

        CraftingRecipe craftingRecipe = GetComponent<CraftingRecipe>();

        // Take the needed materials and the quantites
        string[] needMaterials = craftingRecipe.materials;
        int[] quantity = craftingRecipe.quantity;

        // If the inventory of the player is empty
        if (player.inventory.Count == 0)
        {
            for (int i = 0; i < quantity.Length; i++)
            {
                content += "<color=red> 0/" + quantity[i] + " " + needMaterials[i] + "</color>\n";
            }

            TooltipSystem.Show(content, header);
            return;
        }

        // Verify if every item needed are available
        for (int i = 0; i < needMaterials.Length; i++)
        {
            int index = player.inventory.IndexOf(needMaterials[i]);

            // If the player has not the item
            if (!player.inventory.Contains(needMaterials[i]))
            {
                content += "<color=red> 0/" + quantity[i] + " " + needMaterials[i] + "</color>\n";
            }
            // If the player has the item but not enough
            else if (player.amount[index] < quantity[i])
            {
                content += "<color=red>" + player.amount[index] + "/" + quantity[i] + " " + needMaterials[i] + "</color>\n";
            }
            // If the player has enough of the item
            else
            {
                content += "<color=green>" + player.amount[index] + "/" + quantity[i] + " " + needMaterials[i] + "</color>\n";
            }
        }

        TooltipSystem.Show(content, header);
    }
}
