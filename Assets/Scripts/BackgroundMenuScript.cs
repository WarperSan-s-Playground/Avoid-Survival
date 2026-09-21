using UnityEngine;

public class BackgroundMenuScript : MonoBehaviour
{
    public BoxCollider2D moveArea;
    private Vector3 randPos;
    [SerializeField] private float speed;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        randPos = randomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, randPos) <= 0.01)
        {
            randPos = randomPosition();
        }
        else
        {
            Debug.DrawLine(transform.position, randPos);
            transform.position = Vector3.SmoothDamp(transform.position, randPos,ref velocity, speed);
        }
    }

    private Vector3 randomPosition()
    {
        Vector3 chosenPos = Vector2.zero;

        float randX = Random.Range(moveArea.bounds.center.x - moveArea.bounds.extents.x, moveArea.bounds.center.x + moveArea.bounds.extents.x);
        float randY = Random.Range(moveArea.bounds.center.y - moveArea.bounds.extents.y, moveArea.bounds.center.y + moveArea.bounds.extents.y);

        chosenPos = new Vector3(randX, randY, -1f);

        return chosenPos;
    }
}
