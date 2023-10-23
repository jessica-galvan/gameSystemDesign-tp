using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "PrefabsReferences", menuName = "TP/General/PrefabsReferences", order = 4)]
public class PrefabsReferencesSO : ScriptableObject
{
    [Header("Base")]
    public EventSystem eventSystemPrefab;
    public CameraController camControllerPrefab;
    public CinemachineVirtualCamera virtualCamera;
    public Camera uiCamera;

    [Header("Managers")]
    public UpdateManager updateManagerPrefab;
    public AudioManager audioManagerPrefab;

    [Header("UI")]
    public HUDManager hudManagerPrefab;
    public PauseMenu pauseMenuPrefab;
    public LevelUpPanel levelUpPanelPrefab;
    public UIEffects uiEffectsPrefab;

    [Header("Characters")]
    public PlayerController playerPrefab;
    public EnemyController enemyPrefab;

    [Header("VFX")]
    public ParticleEffect deathVFX;

    [Header("Objects")]
    public ProjectileController playerBasicAttackPrefab;
    public ManaDrop manaDropPrefab;
    //TODO make death prefab and particles?
}
