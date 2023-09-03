using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PrefabsReferences", menuName = "TP/PrefabsReferences", order = 4)]
public class PrefabsReferencesSO : ScriptableObject
{
    [Header("Base")]
    public EventSystem eventSystemPrefab;
    public CameraController camControllerPrefab;

    [Header("Managers")]
    public UpdateManager updateManagerPrefab;
    public AudioManager audioManagerPrefab;

    [Header("UI")]
    public HUDManager hudManagerPrefab;
    public PauseMenu pauseMenuPrefab;

    [Header("Characters")]
    public PlayerController playerPrefab;
    public EnemyController enemyPrefab;

    [Header("Objects")]
    public ProjectileController playerBasicAttackPrefab;
    public ManaDrop manaDropPrefab;
    //TODO make death prefab and particles?
}
