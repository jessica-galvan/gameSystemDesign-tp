using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [Header("References")]
    public HUDManager hudManager;
    public PauseMenu pauseMenu;
    public LevelUpPanel levelUpPanel;

    public void Initialize()
    {
        hudManager = Instantiate(GameManager.Instance.prefabReferences.hudManagerPrefab);
        hudManager.Initialize();

        levelUpPanel = Instantiate(GameManager.Instance.prefabReferences.levelUpPanelPrefab);
        levelUpPanel.Initialize();

        pauseMenu = Instantiate(GameManager.Instance.prefabReferences.pauseMenuPrefab);
        pauseMenu.Initialize();

        hudManager.Open();
        levelUpPanel.Close();
        pauseMenu.Close();

        GameManager.Instance.OnPause += OnPause;
        GameManager.Instance.experienceSystem.OnUpdateLevel += LevelUp;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= OnPause;
        GameManager.Instance.experienceSystem.OnUpdateLevel -= LevelUp;
    }

    public void OnPause(bool isPaused)
    {
        if (isPaused)
        {
            hudManager.Close();
            pauseMenu.Open();
        }
        else
        {
            hudManager.Open();
            pauseMenu.Close();
        }
    }

    public void LevelUp(int currentLevel)
    {
        if (!GameManager.Instance.Player.CanUnlockAbility() && !ScriptableObjectManager.Instance.HasPowerUpsToUnlock(GameManager.Instance.playerData.maxPowerUpSelection)) return;
        hudManager.UpdateLevel(currentLevel);
        levelUpPanel.Open();
    }
}
