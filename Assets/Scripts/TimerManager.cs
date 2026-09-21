using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [Header("Timer Related")]
    public Text timerText;
    public int InitialCountDownValue;
    private bool hardcore;
    [Space]
    public MovingCharacter player;
    public Animator gameWonAnimator;
    public GameObject[] monsterList;
    [SerializeField] private float[] monsterSpawningChances;

    public Transform[,] points;
    public Transform[] spawningAreas;

    // Start is called before the first frame update
    void Start()
    {
        if (timerText.gameObject == null)
        {
            Debug.Log("No Timer Text");
            Destroy(gameObject);
            return;
        }

        // Set the local variable for hardcore to the global one
        hardcore = DifficultyManagerScript.hardcoreModeActivated;

        if (hardcore)
        {
            timerText.color = Color.red;
        }

        InitialCountDownValue = Mathf.FloorToInt(DifficultyManagerScript.totalTime);

        if (DifficultyManagerScript.protectTargetActivated)
        {
            points = new Transform[spawningAreas.Length, 2];

            // For each spawning area, get each corner
            for (int i = 0; i < spawningAreas.Length; i++)
            {
                points[i, 0] = spawningAreas[i].GetChild(0);
                points[i, 1] = spawningAreas[i].GetChild(1);
            }

        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null)
        {
            Debug.Log("No player found");
            return;
        }

        player = playerObj.GetComponent<MovingCharacter>();

        StartCoroutine("StartCountdown");
    }

    public IEnumerator StartCountdown()
    {
        //if (MapManager.instance.survivalGame)
        //{
        //    int currentCountDownValue = 0;

        //    while (player.gameEnded == false)
        //    {
        //        timerText.text = currentCountDownValue + " seconds";
        //        yield return new WaitForSeconds(1.0f);
        //        currentCountDownValue++;

        //        int monsterIndex = 0;
        //        for (int i = monsterList.Length - 1; i > 0; i--)
        //        {
        //            if (Random.value <= monsterSpawningChances[i] / 100)
        //            {
        //                monsterIndex = i;
        //                i = 0;
        //            }
        //        }

        //        // If 1/6 of the total time passed  or hardcore is on
        //        if (InitialCountDownValue / 6 >= currentCountDownValue || hardcore)
        //        {
        //            // In 40% of the cases, spawn
        //            if (Random.value <= 0.4f)
        //            {
        //                int randomIndex = 0;

        //                // If Protect The Target is activated
        //                if (DifficultyManagerScript.protectTargetActivated)
        //                {
        //                    randomIndex = Random.Range(0, spawningAreas.Length);
        //                }
        //                else
        //                {
        //                    randomIndex = Random.Range(0, MapManager.instance.spawningAreas.Length);
        //                    points = MapManager.instance.points;
        //                }

        //                float randX = Random.Range(points[randomIndex, 0].position.x, points[randomIndex, 1].position.x);
        //                float randY = Random.Range(points[randomIndex, 0].position.y, points[randomIndex, 1].position.y);

        //                if (Physics2D.OverlapCircleAll(new Vector2(randX, randY), 0.3f).Length == 0)
        //                {
        //                    Instantiate(monsterList[monsterIndex], new Vector2(randX, randY), Quaternion.identity);
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
            int currentCountDownValue = InitialCountDownValue;

            while (currentCountDownValue > -1 && player.gameEnded == false)
            {
                timerText.text = "Timer: " + currentCountDownValue + " seconds";
                yield return new WaitForSeconds(1.0f);
                currentCountDownValue--;

                int monsterIndex = 0;
                for (int i = monsterList.Length - 1; i > 0; i--)
                {
                    if (Random.value <= monsterSpawningChances[i] / 100)
                    {
                        monsterIndex = i;
                        i = 0;
                    }
                }

                // If 1/6 of the total time passed  or hardcore is on
                if (InitialCountDownValue - InitialCountDownValue / 6 >= currentCountDownValue || hardcore)
                {
                    // In 40% of the cases, spawn
                    if (Random.value <= 0.4f)
                    {
                        int randomIndex = 0;

                        // If Protect The Target is activated
                        if (DifficultyManagerScript.protectTargetActivated)
                        {
                            randomIndex = Random.Range(0, spawningAreas.Length);
                        }
                        else
                        {
                            randomIndex = Random.Range(0, MapManager.instance.spawningAreas.Length);
                            points = MapManager.instance.points;
                        }

                        float randX = Random.Range(points[randomIndex, 0].position.x, points[randomIndex, 1].position.x);
                        float randY = Random.Range(points[randomIndex, 0].position.y, points[randomIndex, 1].position.y);

                        if (Physics2D.OverlapCircleAll(new Vector2(randX, randY), 0.3f).Length == 0)
                        {
                            Instantiate(monsterList[monsterIndex], new Vector2(randX, randY), Quaternion.identity);
                        }
                    }
                }
            }
        //}

        if (player.gameEnded)
            yield break;

        // End Game

        player.gameEnded = true;
        gameWonAnimator.Play("GameOverMenu");
        foreach (var ennemy in GameObject.FindGameObjectsWithTag("Ennemy"))
        {
            Destroy(ennemy);
        }
    }
}
