using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "TP/GlobalConfig", order = 1)]
public class GlobalConfigSO : ScriptableObject
{
    [Header("Scenes")]
    public string gameScene = "Gameplay";
    public string menuScene = "MainMenu";

    [Header("Gameplay")]
    public int maxEnemiesAtAllTimes = 10;
    public float minSpawnTime = 0;
    public float maxSpawnTime = 5f;

    [Header("CustomUpdate Settings")]
    [Tooltip("This FrameRate is for the gameplay things that keep adding and leaving: bullets, enemies, etc.")]
    public int gameplayFPSTarget = 60; //nothign that depends on the input system should be here

    [Tooltip("This FrameRate is for the UI")]
    public int uiFPSTarget = 60;

    [Header("HUD")]
    public string pointsFormat = "00000";
    public string timeFormat = "mm':'ss";

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

}
