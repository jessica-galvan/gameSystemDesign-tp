using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PrefabsReferences", menuName = "TP/PrefabsReferences", order = 4)]
public class PrefabsReferencesSO : ScriptableObject
{
    [Header("Base")]
    public EventSystem eventSystemPrefab;
    public UpdateManager updateManagerPrefab;

    [Header("UI")]
    public HUDManager hudManagerPrefab;
    public PauseMenu pauseMenuPrefab;

    [Header("Characters")]
    public PlayerController playerPrefab;
    public ProjectileController testingBulletPrefab;
    //TODO make death prefab and pool 
}
