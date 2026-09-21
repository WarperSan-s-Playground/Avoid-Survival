using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas levelMenu;
    [SerializeField] private Canvas mainMenu;

    public void LoadLevelMenu()
    {
        levelMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        levelMenu.gameObject.SetActive(false);
    }
}
