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
    public Transform playerSpawningPoint;

    [Header("Info")]
    [ReadOnly] public AudioManager audioManager;
    [ReadOnly] public UpdateManager updateManager;
    [ReadOnly] public GameplayUIManager gameplayUIManager;
    //TODO EnemyManager with PoolSystem
    //TODO HUDManager

    [field: SerializeField, ReadOnly] public PlayerModel Player { get; private set; }
    [field: SerializeField, ReadOnly] public bool Pause { get; private set; }
    [field: SerializeField, ReadOnly] public bool Won { get;private set; }
    [field: SerializeField, ReadOnly] public bool CanUpdate => !Won && !Pause;

    public GameInputs Input { get; private set; }

    public Action<bool> OnPause;
    public Action OnWin;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Time.timeScale = 1;
        Instance = this;

        Input = new GameInputs();
        Input.Gameplay.Enable();
        Input.Gameplay.Pause.performed += TogglePause;
        Input.Menu.Resume.performed += TogglePause;

        Instantiate(prefabReferences.eventSystemPrefab);
        audioManager = Instantiate(prefabReferences.audioManagerPrefab);

        updateManager = Instantiate(prefabReferences.updateManagerPrefab);
        updateManager.Initialize();

        var player = Instantiate(prefabReferences.playerPrefab);
        player.Initialize();
        Player = player.Model;
        Player.Initialize();

        var enemy = Instantiate(prefabReferences.enemyPrefab);
        enemy.transform.position = playerSpawningPoint.position;
        enemy.Initialize();

        gameplayUIManager = gameObject.AddComponent<GameplayUIManager>();
        gameplayUIManager.Initialize();
    }

    private void OnDestroy()
    {
        Input.Gameplay.Pause.performed -= TogglePause;
        Input.Menu.Resume.performed -= TogglePause;
    }

    private void TogglePause(InputAction.CallbackContext cxt)
    {
        if (Won) return;
        TogglePause();
    }

    public void SetPause(bool value)
    {
        if (Pause == value) return;

        if (value)
        {
            Input.Gameplay.Disable();
            Input.Menu.Enable();
        }
        else
        {
            Input.Gameplay.Enable();
            Input.Menu.Disable();
        }



        Pause = value;
        Time.timeScale = Pause ? 0 : 1;
        OnPause.Invoke(Pause);
    }

    private void TogglePause()
    {
        if (Won) return;
        SetPause(!Pause);
    }

    public void WinGame()
    {
        if (Won) return;
        Won = true;
        Pause = true;

        audioManager.PlaySFXSound(soundReferences.win);
        OnWin.Invoke();
    }
}
