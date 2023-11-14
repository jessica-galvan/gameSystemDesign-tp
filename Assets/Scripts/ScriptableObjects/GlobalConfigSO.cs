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
    public int initialPool = 10;

    [Header("Enemy Spawn Logic")]
    public float gameplayDuration = 600f;
    public EnemyStage[] allStages = new EnemyStage[0];
    public EnemyStage lastStage;
    public int enemyInitialPool = 5;
    public bool respawnEnemiesWhenTooFar = true;
    [Tooltip("When not inside this area, that centers at the player's position, the enemies will be respawned")]
    public Vector2 invisibleCollision = new Vector2(5f, 10f);
    public EnemyController[] allEnemies = new EnemyController[0];

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
        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].Stats.Initialize();
        }
    }

    public void ScaleDifficulty()
    {
        //MaxSpawnedAmount += Mathf.RoundToInt(MaxSpawnedAmount * maxAmountPerSpawnMultiplier);
        //MaxEnemiesAmount += Mathf.RoundToInt(MaxEnemiesAmount * maxEnemiesAmountMultiplier);

        //for (int i = 0; i < allEnemies.Length; i++)
        //    allEnemies[i].ScaleUpDifficulty();
    }

    public bool CanScaleDifficult(int currentLevel)
    {
        return currentLevel % amountLevelsUpDifficulty == 0;
    }
}
