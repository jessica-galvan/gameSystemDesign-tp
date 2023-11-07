using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [Header("References")]
    public HUDManager hudManager;
    public SpecialScreensManager specialScreensManager;
    public LevelUpPanel levelUpPanel;

    public void Initialize()
    {
        hudManager = Instantiate(GameManager.Instance.prefabReferences.hudManagerPrefab);
        hudManager.Initialize();

        levelUpPanel = Instantiate(GameManager.Instance.prefabReferences.levelUpPanelPrefab);
        levelUpPanel.Initialize();

        specialScreensManager = Instantiate(GameManager.Instance.prefabReferences.specialScreensManagerPrefab);
        specialScreensManager.Initialize();

        hudManager.Open();
        levelUpPanel.Close();

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
            specialScreensManager.PauseMenu.Open();
        }
        else
        {
            hudManager.Open();
            specialScreensManager.PauseMenu.Close();
        }
    }

    public void LevelUp(int currentLevel)
    {
        if (!GameManager.Instance.Player.CanUnlockAbility() && !ScriptableObjectManager.Instance.HasPowerUpsToUnlock(GameManager.Instance.playerData.maxPowerUpSelection)) return;
        hudManager.UpdateLevel(currentLevel);
        levelUpPanel.Open();
    }
}
