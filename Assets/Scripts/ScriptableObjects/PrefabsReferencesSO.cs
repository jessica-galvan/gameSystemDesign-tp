using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PrefabsReferences", menuName = "TP/PrefabsReferences", order = 4)]
public class PrefabsReferencesSO : ScriptableObject
{
    [Header("Base")]
    public EventSystem eventSystemPrefab;

    [Header("Managers")]
    public UpdateManager updateManagerPrefab;
    public AudioManager audioManagerPrefab;
    public EnemyManager enemyManagerPrefab;
    //TODO do pool manager

    [Header("UI")]
    public HUDManager hudManagerPrefab;
    public PauseMenu pauseMenuPrefab;

    [Header("Characters")]
    public PlayerController playerPrefab;
    public ProjectileController testingBulletPrefab;
    public EnemyController enemyPrefab;
    //TODO make death prefab and particles?
}
