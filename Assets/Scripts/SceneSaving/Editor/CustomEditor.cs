using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEditor : EditorWindow
{
    public static float totalTime;
    [Header("Modes")]
    public static bool hardcoreModeActivated = false; // Hardcore mode on/off
    public static bool staminaModeActivated; // If the player has no more energy, it dies
    public static bool protectTargetActivated; // Protect a target gamemode activated

    [Header("Hardcore")]
    public static float hardcoreSpeedModifier = 1f; // Hardcore mode speed modifier (enemies)
    public static float hardcoreHealthModifier = 1f; // Hardcore mode health modifier (enemies)

    [Header("Player Data")]
    public static bool infiniteSprinting; // Infinite sprinting (player)
    public static bool infiniteHealth; // Infinite health (player)
    public static float playerBaseHealth; // Base health (player)
    public static float playerWalkingSpeed; // Walking Speed (player)
    public static float playerRunningSpeed; // Running Speed (player)
    public static float defaultShieldHealth; // Defaut health of the shield
    public static bool startWithShield; // Start the game with the shield on or off
    public static float energyGainRate;
    public static float energyLostRate;

    [Header("Map Generation Settings")]
    public static int amountTree; // Amount of Trees
    public static int amountRock; // Amount of Rocks
    public static int amountIronOre; // Amount of Iron Ores

    private Vector2 scrollPos = Vector2.zero;
    private string sceneName = "";
    [MenuItem("Level/Settings")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditor>("LevelSettings");
    }

    void OnGUI()
    {
        float space = 20f;

        if (GUILayout.Button("Save Level Settings"))
        {
            SaveFile();
        }

        if (GUILayout.Button("Load Level Settings"))
        {
            scrollPos = Vector2.zero;

            sceneName = SceneManager.GetActiveScene().name;

            LoadFile();
        }

        if (sceneName != "")
            EditorGUILayout.LabelField(sceneName + " Settings", EditorStyles.boldLabel);

        Rect rectPos = EditorGUILayout.GetControlRect();
        Rect rectBox = new Rect(rectPos.x, rectPos.y, rectPos.width, 500f);

        Rect viewRect = new Rect(rectBox.x - 10f, rectBox.y, rectBox.width, rectBox.height * 5 / 3);

        scrollPos = GUI.BeginScrollView(rectBox, scrollPos, viewRect, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);

        EditorGUILayout.LabelField("Total Time");
        totalTime = EditorGUILayout.Slider(totalTime, 1f, 200f);

        EditorGUILayout.LabelField("Game Modes");
        protectTargetActivated = EditorGUILayout.Toggle("Protect the Target Mode", protectTargetActivated);

        // Hardcore
        hardcoreModeActivated = EditorGUILayout.Toggle("Hardcore Mode", hardcoreModeActivated);

        if (hardcoreModeActivated == true)
        {
            EditorGUILayout.LabelField("Speed Modifier");
            hardcoreSpeedModifier = EditorGUILayout.Slider(hardcoreSpeedModifier, -10f, 10f);

            EditorGUILayout.LabelField("Health Modifier");
            hardcoreHealthModifier = EditorGUILayout.Slider(hardcoreHealthModifier, -10f, 10f);

            EditorGUILayout.Space(space);
        }

        staminaModeActivated = EditorGUILayout.Toggle("Stamina Mode", staminaModeActivated);

        // Player Data
        infiniteSprinting = EditorGUILayout.Toggle("Infinite Energy", infiniteSprinting);

        if (infiniteSprinting == false)
        {
            EditorGUILayout.LabelField("Energy Gain Rate");
            energyGainRate = EditorGUILayout.Slider(energyGainRate, -10f, 10f);

            EditorGUILayout.LabelField("Energy Lost Rate");
            energyLostRate = EditorGUILayout.Slider(energyLostRate, -10f, 10f);

            EditorGUILayout.Space(space);
        }

        EditorGUILayout.LabelField("Player Walking Speed");
        playerWalkingSpeed = EditorGUILayout.Slider(playerWalkingSpeed, -10f, 10f);

        EditorGUILayout.LabelField("Player Running Speed");
        playerRunningSpeed = EditorGUILayout.Slider(playerRunningSpeed, -10f, 10f);

        infiniteHealth = EditorGUILayout.Toggle("Infinite Health", infiniteHealth);

        if (infiniteHealth == false)
        {
            EditorGUILayout.LabelField("Base Player Health");
            playerBaseHealth = EditorGUILayout.Slider(playerBaseHealth, 1f, 100f);

            EditorGUILayout.Space(space);
        }

        startWithShield = EditorGUILayout.Toggle("Start with Shield", startWithShield);

        EditorGUILayout.LabelField("Default Shield Health");
        defaultShieldHealth = EditorGUILayout.Slider(defaultShieldHealth, 1f, 100f);

        EditorGUILayout.Space(space);
        EditorGUILayout.LabelField("Map Generation");

        EditorGUILayout.LabelField("Amount of Tree");
        amountTree = Mathf.FloorToInt(EditorGUILayout.Slider(amountTree, 0f, 60f));

        EditorGUILayout.LabelField("Amount of Rock");
        amountRock = Mathf.FloorToInt(EditorGUILayout.Slider(amountRock, 0f, 60f));

        EditorGUILayout.LabelField("Amount of Iron Ore");
        amountIronOre = Mathf.FloorToInt(EditorGUILayout.Slider(amountIronOre, 0f, 60f));

        GUI.EndScrollView();
    }

    public static void SaveFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/ScenesSettings/" + SceneManager.GetActiveScene().name + ".dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        if (!File.Exists(path))
        {
            File.Create(path);
        }

        SceneData data = new SceneData(totalTime,
            hardcoreModeActivated,
            staminaModeActivated,
            protectTargetActivated,
            hardcoreSpeedModifier,
            hardcoreHealthModifier,
            infiniteSprinting,
            infiniteHealth,
            playerBaseHealth,
            playerWalkingSpeed,
            playerRunningSpeed,
            defaultShieldHealth,
            startWithShield,
            amountTree,
            amountRock,
            amountIronOre,
            energyGainRate,
            energyLostRate);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log(SceneManager.GetActiveScene().name + " saved !");
    }

    public static void LoadFile()
    {
        string path = Application.dataPath + "/ScenesSettings/" + SceneManager.GetActiveScene().name + ".dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SceneData data = formatter.Deserialize(stream) as SceneData;
            stream.Close();

            totalTime = data.totalTime;

            // Modes
            hardcoreModeActivated = data.hardcoreModeActivated;
            staminaModeActivated = data.staminaModeActivated;
            protectTargetActivated = data.protectTargetActivated;

            // Hardcore
            hardcoreSpeedModifier = data.hardcoreSpeedModifier;
            hardcoreHealthModifier = data.hardcoreHealthModifier;

            // Player Data
            infiniteSprinting = data.infiniteSprinting;
            energyGainRate = data.energyGainRate;
            energyLostRate = data.energyLostRate;

            playerWalkingSpeed = data.playerWalkingSpeed;
            playerRunningSpeed = data.playerRunningSpeed;

            infiniteHealth = data.infiniteHealth;
            playerBaseHealth = data.playerBaseHealth;

            startWithShield = data.startWithShield;
            defaultShieldHealth = data.defaultShieldHealth;

            // Map Generation
            amountTree = data.amountTree;
            amountRock = data.amountRock;
            amountIronOre = data.amountIronOre;
        }
        else
        {
            Debug.LogError("File Not Found");
        }
    }
}