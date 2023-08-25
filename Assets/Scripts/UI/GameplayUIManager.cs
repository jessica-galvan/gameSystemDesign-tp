using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [Header("References")]
    public HUDManager hudManager;
    public PauseMenu pauseMenu;

    public void Initialize()
    {
        hudManager = Instantiate(GameManager.Instance.prefabReferences.hudManagerPrefab);
        hudManager.Initialize();

        //TODO add power up selection menu
        pauseMenu = Instantiate(GameManager.Instance.prefabReferences.pauseMenuPrefab);

        hudManager.Open();
        pauseMenu.Close();

        GameManager.Instance.OnPause += OnPause;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= OnPause;
    }

    private void OnPause(bool isPaused)
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
}
