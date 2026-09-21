using System.Collections.Generic;
using UnityEngine;

public class DifficultyManagerScript : MonoBehaviour
{
    public static float totalTime = 120f;

    [Header("Modes")]
    public static bool hardcoreModeActivated = false; // Hardcore mode on/off
    public static bool staminaModeActivated = false; // If the player has no more energy, it dies
    public static bool protectTargetActivated = false; // Protect a target gamemode activated

    [Header("Hardcore")]
    public static float hardcoreSpeedModifier = 1f; // Hardcore mode speed modifier (enemies)
    public static float hardcoreHealthModifier = 1f; // Hardcore mode health modifier (enemies)

    [Header("Player Data")]
    public static bool infiniteSprinting = false; // Infinite sprinting (player)
    public static bool infiniteHealth = false; // Infinite health (player)
    public static float playerBaseHealth = 10f; // Base health (player)
    public static float playerWalkingSpeed = 1f; // Walking Speed (player)
    public static float playerRunningSpeed = 1.5f; // Running Speed (player)
    public static float defaultShieldHealth = 5f; // Defaut health of the shield
    public static bool startWithShield = false; // Start the game with the shield on or off
    public static float energyGainRate = .3f;
    public static float energyLostRate = 1f;

    [Header("Map Generation Settings")]
    public static string selectedMap;
    public static List<string> possibleMaps = new List<string> {
        "GreenValley",
        "DarkCave",
        "SoopaBeach",
        "BoBombaFactory",
        "TickTickParty",
        "SurviveAmongUs"
    };

    public static Dictionary<string, float> materialsChanceDrop = new Dictionary<string, float>() {
        { "Planks", 0.8f },
        {"Stone", 0.5f },
        {"Iron Ingots", 0.35f}
    };

    public static int amountTree = 60; // Amount of Trees
    public static int amountRock = 40; // Amount of Rocks
    public static int amountIronOre = 30; // Amount of Iron Ores
}
