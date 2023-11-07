using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScreensManager : MonoBehaviour
{
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private PauseMenu pauseMenuPanel;

    public GameOverPanel GameOverPanel => gameOverPanel;
    public PauseMenu PauseMenu => pauseMenuPanel;

    public void Initialize()
    {
        pauseMenuPanel.Initialize();
        pauseMenuPanel.Close();

        gameOverPanel.Initialize();
        gameOverPanel.Close();
    }
}
