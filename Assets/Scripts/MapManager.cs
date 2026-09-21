using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform[,] points;
    public Transform[] spawningAreas;

    [Header("Generation Settings")]
    public int numberOfTrees;
    public GameObject treePrefab;
    public int numberOfRocks;
    public GameObject rockPrefab;
    public int numberOfIronOre;
    public GameObject IronOrePrefab;

    [SerializeField] private float radius;

    public static MapManager instance;
    public bool survivalGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.Log("More than one Map Manager");
            return;
        }

        instance = this;

        points = new Transform[spawningAreas.Length, 2];

        // For each spawning area, get each corner
        for (int i = 0; i < spawningAreas.Length; i++)
        {
            points[i, 0] = spawningAreas[i].GetChild(0);
            points[i, 1] = spawningAreas[i].GetChild(1);
        }

        numberOfTrees = DifficultyManagerScript.amountTree;
        numberOfRocks = DifficultyManagerScript.amountRock;
        numberOfIronOre = DifficultyManagerScript.amountIronOre;

        MapGeneration(numberOfTrees, treePrefab);
        MapGeneration(numberOfRocks, rockPrefab);
        MapGeneration(numberOfIronOre, IronOrePrefab);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        DestroyImmediate(player.transform.Find("StartingZone").gameObject, true);
    }

    private void MapGeneration(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            // Select a random spawning area
            int randomIndex = Random.Range(0, spawningAreas.Length);

            float randX = Random.Range(points[randomIndex, 0].position.x, points[randomIndex, 1].position.x);
            float randY = Random.Range(points[randomIndex, 0].position.y, points[randomIndex, 1].position.y);

            Vector2 randPos = new Vector2(randX, randY);

            if (!wallCheck(Physics2D.OverlapCircleAll(randPos, radius)))
            {
                GameObject objectSpawned = Instantiate(prefab, new Vector2(randX, randY), Quaternion.identity);

                Vector2 pos = objectSpawned.transform.position;

                objectSpawned.transform.position = new Vector3(pos.x, pos.y, Mathf.Abs(pos.y));
            }
        }
    }

    private bool wallCheck(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
