using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "TP/GlobalConfig", order = 1)]
public class GlobalConfigSO : ScriptableObject
{
    [Header("CustomUpdate Settings")]
    [Tooltip("This FrameRate is for the gameplay things that keep adding and leaving: bullets, enemies, etc.")]
    public int gameplayFPSTarget = 60; //nothign that depends on the input system should be here

    [Tooltip("This FrameRate is for the UI")]
    public int uiFPSTarget = 60;
}
