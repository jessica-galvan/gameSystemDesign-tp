using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    [Header("References")]
    public GlobalConfigSO globalConfig;
    public PrefabsReferencesSO prefabReferences;
    public Transform playerSpawningPoint;

    [Header("Info")]
    [ReadOnly] public UpdateManager updateManager;
    //TODO EnemyManager with PoolSystem
    //TODO HUDManager

    [field: SerializeField, ReadOnly] public PlayerModel Player { get; private set; }
    [field: SerializeField, ReadOnly] public bool Pause { get; private set; }
    [field: SerializeField, ReadOnly] public bool Won { get;private set; }

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

        Instance = this;

        Input = new GameInputs();

        updateManager = Instantiate(prefabReferences.updateManagerPrefab);
        updateManager.Initialize();
    }

    public void SetPause(bool value)
    {
        if (Pause == value) return;

        Pause = value;
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

        AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.win);
        OnWin.Invoke();
    }
}
