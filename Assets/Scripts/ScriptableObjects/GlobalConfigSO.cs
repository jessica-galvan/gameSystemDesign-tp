using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "TP/General/GlobalConfig", order = 1)]
public class GlobalConfigSO : ScriptableObject
{
    [Header("Scenes")]
    public string gameScene = "Gameplay";
    public string menuScene = "MainMenu";

    [Header("Gameplay")]
    [Tooltip("Amount of levels the player can level up to. If 0, it will level upt infinetly")]
    public int maxLevelCap = 0;
    public int maxEnemiesAtAllTimes = 10;
    public int maxAmountPerSpawn = 5;
    public float minSpawnTime = 0;
    public float maxSpawnTime = 5f;
    public int initialPool = 10;

    [Header("Overtime changes")]
    [Tooltip("Every this amount of levels, the overtime changes will be applied")]
    public int amountLevelsUpDifficulty = 5;
    public float maxEnemiesAmountMultiplier = 0.1f;
    public float maxAmountPerSpawnMultiplier = 0.1f;

    [Header("CustomUpdate Settings")]
    [Tooltip("This FrameRate is for the gameplay things that keep adding and leaving: bullets, enemies, etc.")]
    public int gameplayFPSTarget = 60; //nothign that depends on the input system should be here

    [Tooltip("This FrameRate is for the UI")]
    public int uiFPSTarget = 60;

    [Header("HUD")]
    public string pointsFormat = "00000";
    public string timeFormat = "mm':'ss";
    public Sprite[] keyAbilitiesSprite;

    [Serializable]
    public class PopupInfo
    {
        public string title;
        [TextArea] public string description;
    }

    [Header("Texts")]
    public PopupInfo restartPopup;
    public PopupInfo mainMenuPopup;
    public PopupInfo exitPopup;

    public bool HasLevelCap => maxLevelCap > 0;
    public int MaxSpawnedAmount { get; private set; }
    public int MaxEnemiesAmount { get; private set; }

    public void Initialize()
    {
        MaxSpawnedAmount = maxAmountPerSpawn;
        MaxEnemiesAmount = maxEnemiesAtAllTimes;
    }

    public void ScaleDifficulty()
    {
        MaxSpawnedAmount += Mathf.RoundToInt(MaxSpawnedAmount * maxAmountPerSpawnMultiplier);
        MaxEnemiesAmount += Mathf.RoundToInt(MaxEnemiesAmount * maxEnemiesAmountMultiplier);
    }

    public bool CanScaleDifficult(int currentLevel)
    {
        return currentLevel % amountLevelsUpDifficulty == 0;
    }
}
