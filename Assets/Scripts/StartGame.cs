using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Space]
    [SerializeField] private TMP_Dropdown mapSelectionDropdown;

    [Header("Hardcore")]
    [SerializeField] private Toggle hardcoreToggle;
    [SerializeField] private TMP_InputField hardcoreSpeedText;
    [SerializeField] private TMP_InputField hardcoreHealthText;

    [Header("Player Data")]
    [SerializeField] private Toggle sprintToggle;
    [SerializeField] private Toggle healthToggle;
    [SerializeField] private TMP_InputField playerBaseHealthText;
    [SerializeField] private TMP_InputField playerWalkingSpeedText;
    [SerializeField] private TMP_InputField playerRunningSpeedText;
    [SerializeField] private Toggle startWithShieldToggle;
    [SerializeField] private TMP_InputField defaultShieldHealth;
    [SerializeField] private Slider energyGainRateSlider;
    [SerializeField] private Slider energyLostRateSlider;

    [Header("Map Generation Settings")]
    [SerializeField] private Slider amountTreeSlider;
    [SerializeField] private Slider chanceDropTree;
    [SerializeField] private Slider amountRockSlider;
    [SerializeField] private Slider chanceDropRock;
    [SerializeField] private Slider amountIronOreSlider;
    [SerializeField] private Slider chanceDropIronOre;

    private string currentSelectedMap;
    private GameObject currentMapButton = null;

    public void SelectMap(GameObject button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        currentSelectedMap = text.text.Replace(" ", "");

        text.color = Color.green;

        if (currentMapButton != null)
        {
            currentMapButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        
        if (currentMapButton == button)
        {
            currentMapButton = null;
        }
        else
        {
            currentMapButton = button;
        }
    }

    public void startGame(bool customPlay)
    {
        if (!customPlay)
        {
            if (string.IsNullOrEmpty(currentSelectedMap))
                return;

            // If random is selected
            if (currentSelectedMap == "Random")
            {
                string sceneName = DifficultyManagerScript.possibleMaps[Random.Range(0, DifficultyManagerScript.possibleMaps.Count)];

                // If the scene has it's settings saved
                if (File.Exists(Application.dataPath + "/ScenesSettings/" + sceneName + ".dat"))
                {
                    LoadFile(Application.dataPath + "/ScenesSettings/" + sceneName + ".dat");

                    DifficultyManagerScript.selectedMap = sceneName;
                }
                // Else use default settings
                else
                {
                    DifficultyManagerScript.totalTime = 120f;

                    // Hardcore
                    DifficultyManagerScript.hardcoreModeActivated = false;
                    DifficultyManagerScript.hardcoreHealthModifier = 1f;
                    DifficultyManagerScript.hardcoreSpeedModifier = 1f;

                    // Player Related
                    DifficultyManagerScript.infiniteSprinting = false;
                    DifficultyManagerScript.infiniteHealth = false;
                    DifficultyManagerScript.playerBaseHealth = 10f;
                    DifficultyManagerScript.playerWalkingSpeed = 1f;
                    DifficultyManagerScript.playerRunningSpeed = 1.5f;
                    DifficultyManagerScript.startWithShield = false;
                    DifficultyManagerScript.defaultShieldHealth = 5f;

                    DifficultyManagerScript.energyGainRate = 0.3f;
                    DifficultyManagerScript.energyLostRate = 1f;

                    // Map Generation
                    DifficultyManagerScript.selectedMap = "GreenValley";

                    DifficultyManagerScript.amountTree = 40;

                    DifficultyManagerScript.amountRock = 15;

                    DifficultyManagerScript.amountIronOre = 15;
                }
            }
            else
            {
                string sceneName = currentSelectedMap;

                if (!DifficultyManagerScript.possibleMaps.Contains(sceneName))
                {
                    Debug.Log("The selected map is not an available map");
                    return;
                }

                string destination = "";

                destination = Application.dataPath + "/ScenesSettings/" + sceneName + ".dat";

                // If the scene has it's settings saved
                if (File.Exists(destination))
                {
                    LoadFile(destination);

                    DifficultyManagerScript.selectedMap = sceneName;
                }
                else
                {
                    Debug.Log("Save File Not Found. Please check if the directory ScenesSettings is indeed in the directory Assets");
                    return;
                }
            }
        }

        SceneManager.LoadScene(DifficultyManagerScript.selectedMap);
    }

    public void moreInfo()
    {
        SceneManager.LoadScene("Infos");
    }

    public void StartCustomGame()
    {
        DifficultyManagerScript.materialsChanceDrop.Clear();

        string selectedMapTemp = mapSelectionDropdown.options[mapSelectionDropdown.value].text.Replace(" ", "");

        // If the selected map is in the maps available
        if (DifficultyManagerScript.possibleMaps.Contains(selectedMapTemp))
        {
            DifficultyManagerScript.selectedMap = selectedMapTemp;
        }
        else
        {
            // If there is no map available
            if (DifficultyManagerScript.possibleMaps.Count == 0)
            {
                Debug.LogError("No map available to play on !");
                return;
            }

            // Select a random map from the available ones
            DifficultyManagerScript.selectedMap = DifficultyManagerScript.possibleMaps[Random.Range(0, DifficultyManagerScript.possibleMaps.Count)];
            Debug.Log(selectedMapTemp + " not available. A random map was assigned");
        }

        // Hardcore Related
        DifficultyManagerScript.hardcoreModeActivated = hardcoreToggle.isOn;

        if (hardcoreSpeedText.text == "")
        {
            DifficultyManagerScript.hardcoreHealthModifier = 1f;
        }
        else
        {
            float.TryParse(hardcoreSpeedText.text, out float result);
            DifficultyManagerScript.hardcoreHealthModifier = result;
        }

        if (hardcoreHealthText.text == "")
        {
            DifficultyManagerScript.hardcoreSpeedModifier = 1f;
        }
        else
        {
            float.TryParse(hardcoreSpeedText.text, out float result);
            DifficultyManagerScript.hardcoreSpeedModifier = result;
        }

        // Player Related
        DifficultyManagerScript.infiniteSprinting = sprintToggle.isOn;
        DifficultyManagerScript.infiniteHealth = healthToggle.isOn;
        DifficultyManagerScript.startWithShield = startWithShieldToggle.isOn;

        if (playerBaseHealthText.text == "")
        {
            DifficultyManagerScript.playerBaseHealth = 10f;
        }
        else
        {
            float.TryParse(playerBaseHealthText.text, out float result);

            if (result <= 0)
                result = 1f;

            DifficultyManagerScript.playerBaseHealth = result;
        }

        if (playerWalkingSpeedText.text == "")
        {
            DifficultyManagerScript.playerWalkingSpeed = 1f;
        }
        else
        {
            float.TryParse(playerWalkingSpeedText.text, out float result);
            DifficultyManagerScript.playerWalkingSpeed = result;
        }

        if (playerRunningSpeedText.text == "")
        {
            DifficultyManagerScript.playerRunningSpeed = 1.5f;
        }
        else
        {
            float.TryParse(playerRunningSpeedText.text, out float result);

            DifficultyManagerScript.playerRunningSpeed = result;
        }

        if (defaultShieldHealth.text == "")
        {
            DifficultyManagerScript.defaultShieldHealth = 10f;
        }
        else
        {
            float.TryParse(defaultShieldHealth.text, out float result);

            DifficultyManagerScript.defaultShieldHealth = result;
        }

        DifficultyManagerScript.energyGainRate = energyGainRateSlider.value;
        DifficultyManagerScript.energyLostRate = energyLostRateSlider.value;

        // Map related
        DifficultyManagerScript.amountTree = (int)amountTreeSlider.value;
        DifficultyManagerScript.materialsChanceDrop.Add("Planks", chanceDropTree.value);

        DifficultyManagerScript.amountRock = (int)amountRockSlider.value;
        DifficultyManagerScript.materialsChanceDrop.Add("Stone", chanceDropRock.value);

        DifficultyManagerScript.amountIronOre = (int)amountIronOreSlider.value;
        DifficultyManagerScript.materialsChanceDrop.Add("Iron Ingots", chanceDropIronOre.value);

        startGame(true);
    }

    public void LoadFile(string destination)
    {
        Resources.Load(destination);
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SceneData data = (SceneData)bf.Deserialize(file);
        file.Close();

        DifficultyManagerScript.totalTime = data.totalTime;

        // Modes
        DifficultyManagerScript.hardcoreModeActivated = data.hardcoreModeActivated;
        DifficultyManagerScript.staminaModeActivated = data.staminaModeActivated;
        DifficultyManagerScript.protectTargetActivated = data.protectTargetActivated;

        // Hardcore
        DifficultyManagerScript.hardcoreSpeedModifier = data.hardcoreSpeedModifier;
        DifficultyManagerScript.hardcoreHealthModifier = data.hardcoreHealthModifier;

        // Player Data
        DifficultyManagerScript.infiniteSprinting = data.infiniteSprinting;
        DifficultyManagerScript.infiniteHealth = data.infiniteHealth;
        DifficultyManagerScript.playerBaseHealth = data.playerBaseHealth;
        DifficultyManagerScript.playerWalkingSpeed = data.playerWalkingSpeed;
        DifficultyManagerScript.playerRunningSpeed = data.playerRunningSpeed;

        DifficultyManagerScript.defaultShieldHealth = data.defaultShieldHealth;
        DifficultyManagerScript.startWithShield = data.startWithShield;
        DifficultyManagerScript.energyGainRate = data.energyGainRate;
        DifficultyManagerScript.energyLostRate = data.energyLostRate;

        // Map Generation Settings
        DifficultyManagerScript.amountTree = data.amountTree;
        DifficultyManagerScript.amountRock = data.amountRock;
        DifficultyManagerScript.amountIronOre = data.amountIronOre;

    }
}
