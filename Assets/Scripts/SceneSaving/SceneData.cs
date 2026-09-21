using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public float totalTime;

    [Header("Modes")]
    public bool hardcoreModeActivated; // Hardcore mode on/off
    public bool staminaModeActivated; // If the player has no more energy, it dies
    public bool protectTargetActivated; // Protect a target gamemode activated

    [Header("Hardcore")]
    public float hardcoreSpeedModifier; // Hardcore mode speed modifier (enemies)
    public float hardcoreHealthModifier; // Hardcore mode health modifier (enemies)

    [Header("Player Data")]
    public bool infiniteSprinting; // Infinite sprinting (player)
    public bool infiniteHealth; // Infinite health (player)
    public float playerBaseHealth; // Base health (player)
    public float playerWalkingSpeed; // Walking Speed (player)
    public float playerRunningSpeed; // Running Speed (player)
    public float defaultShieldHealth; // Defaut health of the shield
    public bool startWithShield; // Start the game with the shield on or off
    public float energyGainRate;
    public float energyLostRate;

    [Header("Map Generation Settings")]
    public int amountTree; // Amount of Trees
    public int amountRock; // Amount of Rocks
    public int amountIronOre; // Amount of Iron Ores

    public SceneData(float totalTimeFloat,
        bool hardcoreBool,
        bool staminaModeBool,
        bool protectModeBool,
        float hardcoreSpeedModFloat,
        float hardcoreHealthModFloat,
        bool infiniteSprintingBool,
        bool infiniteHealthBool,
        float playerBaseHealthFloat,
        float playerWalkingSpeedFloat,
        float playerRunningSpeedFloat,
        float defaultShieldHealthFloat,
        bool startWithShieldBool,
        int amountTreeInt,
        int amountRockInt,
        int amountIronOreInt,
        float energyGainRateFloat,
        float energyLostRateFloat)
    {
        totalTime = totalTimeFloat;

        // Modes
        hardcoreModeActivated = hardcoreBool;
        staminaModeActivated = staminaModeBool;
        protectTargetActivated = protectModeBool;

        // Hardcore
        hardcoreSpeedModifier = hardcoreSpeedModFloat;
        hardcoreHealthModifier = hardcoreHealthModFloat;

        // Player Data
        infiniteSprinting = infiniteSprintingBool;
        infiniteHealth = infiniteHealthBool;
        playerBaseHealth = playerBaseHealthFloat;
        playerWalkingSpeed = playerWalkingSpeedFloat;
        playerRunningSpeed = playerRunningSpeedFloat;
        defaultShieldHealth = defaultShieldHealthFloat;
        startWithShield = startWithShieldBool;
        energyGainRate = energyGainRateFloat;
        energyLostRate = energyLostRateFloat;

        // Map Generation Settings
        amountTree = amountTreeInt;
        amountRock = amountRockInt;
        amountIronOre = amountIronOreInt;
    }
}
