using UnityEngine;

public class MainMenuMapGeneration : MonoBehaviour
{
    public BoxCollider2D moveArea;

    [Header("Generation Settings")]
    public int numberOfTrees;
    public GameObject treePrefab;
    public int numberOfRocks;
    public GameObject rockPrefab;
    public int numberOfIronOre;
    public GameObject IronOrePrefab;

    // Start is called before the first frame update
    void Start()
    {
        MapGeneration(numberOfTrees, treePrefab);
        MapGeneration(numberOfRocks, rockPrefab);
        MapGeneration(numberOfIronOre, IronOrePrefab);
    }

    private void MapGeneration(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            float randX = Random.Range(moveArea.bounds.center.x - moveArea.bounds.extents.x, moveArea.bounds.center.x + moveArea.bounds.extents.x);
            float randY = Random.Range(moveArea.bounds.center.y - moveArea.bounds.extents.y, moveArea.bounds.center.y + moveArea.bounds.extents.y);

            Instantiate(prefab, new Vector2(randX, randY), Quaternion.identity);
        }
    }
}
