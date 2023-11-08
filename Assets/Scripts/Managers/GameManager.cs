using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    [Header("References")]
    public GlobalConfigSO globalConfig;
    public PrefabsReferencesSO prefabReferences;
    public SoundReferencesSO soundReferences;
    public PlayerDataSO playerData;

    [Header("Info")]
    [ReadOnly] public CameraController cameraController;
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public GameplayUIManager gameplayUIManager;
    [ReadOnly] public EnemyManager enemyManager;
    [ReadOnly] public PoolManager poolManager;
    [ReadOnly] public UIEffects uiEffects;

    [Header("Systems")]
    [ReadOnly] public ExperienceSystem experienceSystem;
    [ReadOnly] public ManaSystem manaSystem;

    [field: SerializeField, ReadOnly] public PlayerModel Player { get; private set; }
    [field: SerializeField, ReadOnly] public bool Pause { get; private set; }
    [field: SerializeField, ReadOnly] public bool GameOver { get;private set; }

    public bool CanUpdate => !Pause  || !GameOver;
    public GameInputs Input { get; private set; }
    public UIEffects UIEffects => uiEffects;

    public Action<bool> OnPause;
    public Action OnWin;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        Time.timeScale = 1;

        Input = new GameInputs();
        Input.Gameplay.Enable();
        Input.Cheats.Enable();

        globalConfig.Initialize();

        Input.Gameplay.Pause.performed += TogglePause;
        Input.Menu.Resume.performed += TogglePause;

        Instantiate(prefabReferences.eventSystemPrefab);
        audioManager = Instantiate(prefabReferences.audioManagerPrefab);

        updateManager = Instantiate(prefabReferences.updateManagerPrefab);
        updateManager.Initialize();

        var player = Instantiate(prefabReferences.playerPrefab);
        player.Initialize();
        Player = player.Model;
        Player.LifeController.OnDeath += SetGameOver;

        cameraController = Instantiate(prefabReferences.camControllerPrefab);
        cameraController.Initialize();

        uiEffects = GameManager.Instantiate(GameManager.Instance.prefabReferences.uiEffectsPrefab);
        uiEffects.Initialize();

        poolManager = GetComponent<PoolManager>();
        poolManager.Initialize();

        enemyManager = GetComponent<EnemyManager>();
        enemyManager.Initialize();

        experienceSystem = Create<ExperienceSystem>("PlayerSystems");
        experienceSystem.Initialize();

        manaSystem = experienceSystem.AddComponent<ManaSystem>();
        manaSystem.Initialize();

        gameplayUIManager = gameObject.AddComponent<GameplayUIManager>();
        gameplayUIManager.Initialize();
    }

    private void Start()
    {
        ScriptableObjectManager.Instance.Resort();
    }

    public static T Create<T>(string name, Transform parent = null) where T:MonoBehaviour
    {
        var container = new GameObject(name);
        container.transform.parent = parent;
        T newItem = container.AddComponent<T>();
        return newItem;
    }

    private void OnDestroy()
    {
        Input.Gameplay.Pause.performed -= TogglePause;
        Input.Menu.Resume.performed -= TogglePause;
    }

    private void TogglePause(InputAction.CallbackContext cxt)
    {
        if (GameOver) return;
        TogglePause();
    }

    public void SetPause(bool value, bool pauseMenu = true)
    {
        if (Pause == value) return;

        if (value)
        {
            Input.Gameplay.Disable();
            Input.Cheats.Disable();
            Input.Menu.Enable();
        }
        else
        {
            Input.Gameplay.Enable();
            Input.Cheats.Enable();
            Input.Menu.Disable();
        }

        Pause = value;
        Time.timeScale = Pause ? 0 : 1;

        if (pauseMenu)
        {
            gameplayUIManager.OnPause(Pause);
            //OnPause.Invoke(Pause);
        }
    }

    private void TogglePause()
    {
        if (GameOver || gameplayUIManager.levelUpPanel.IsOpen) return;
        SetPause(!Pause);
    }

    public void SetGameOver()
    {
        if (GameOver) return;
        GameOver = true;
        Pause = true;

        gameplayUIManager.specialScreensManager.GameOverPanel.Open();
        //OnWin.Invoke();
    }
}
