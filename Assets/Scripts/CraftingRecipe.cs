using UnityEngine;

public class CraftingRecipe : MonoBehaviour
{
    public string[] materials;
    public int[] quantity;
    public string nameOfItem;
    public string typeOfItem;
    public GameObject blockPrefab = null;
    public Color itemSlotColor;

    public int quantityGiven;
}
