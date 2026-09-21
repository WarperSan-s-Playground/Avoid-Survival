using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.Log("More than one Building Manager");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void BuildingCheck(MovingCharacter player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.blockDisplay.GetComponent<SpriteRenderer>().sprite = null;
            player.blockToPlace = null;
            return;
        }
        else
        {
            SpriteRenderer blockDisplayRender = player.blockDisplay.GetComponent<SpriteRenderer>();

            blockDisplayRender.sprite = player.blockToPlace.GetComponent<SpriteRenderer>().sprite;

            blockDisplayRender.transform.localScale = player.blockToPlace.transform.localScale;

            Vector2 blockPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 blockToPlaceSizes = player.blockToPlace.GetComponent<BoxCollider2D>().size;

            int obstaclesLenght = -1;

            if (player.blockToPlace.GetComponent<BoxCollider2D>() != null)
            {
                obstaclesLenght = Physics2D.OverlapBoxAll(blockPos, blockToPlaceSizes, 0f).Length;
            }

            //If there is nothing to hit
            if (obstaclesLenght > 0)
            {
                blockDisplayRender.color = new Color(1f, 0f, 0f, 0.7f);
            }
            else
            {
                blockDisplayRender.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
            }
        }
    }

    public void Building(MovingCharacter player)
    {
        if (player.blockToPlace == null)
            return;

        string nameOfBlock = player.blockToPlace.GetComponent<BuildingMaterial>().nameOfStructure;

        if (player.inventory.Contains(nameOfBlock) && player.blockToPlace != null)
        {
            if (player.amount[player.inventory.IndexOf(nameOfBlock)] > 0)
            {
                Vector2 blockPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 blockToPlaceSizes = player.blockToPlace.GetComponent<BoxCollider2D>().size;

                Collider2D[] obstacles = Physics2D.OverlapBoxAll(blockPos, blockToPlaceSizes, 0f);

                //If there is nothing to hit
                if (obstacles.Length > 0)
                {
                    Debug.Log("Something is in the way");
                    return;
                }

                if (!player.UpdateSlot(player.inventory.IndexOf(nameOfBlock), -1, nameOfBlock))
                {
                    Debug.Log("Not enough item");
                    return;
                }

                Instantiate(player.blockToPlace, blockPos, Quaternion.identity);

                player.blockToPlace = null;
            }
        }
    }
}
