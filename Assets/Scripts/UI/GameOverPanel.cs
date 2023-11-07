using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameOverPanel : Panel
{
    [SerializeField] private MenuButton restartButton;
    [SerializeField] private MenuButton menuButton;
    [SerializeField] private MenuButton quitButton;

    public override void Initialize()
    {
        base.Initialize();

        restartButton.Button.onClick.AddListener(OnRestartButtonClick);
        restartButton.Deselect();

        menuButton.Button.onClick.AddListener(OnMenuButtonClick);
        menuButton.Deselect();

        quitButton.Button.onClick.AddListener(OnQuitButtonClick);
        quitButton.Deselect();
    }

    public override void Open()
    {
        base.Open();
        //audioManager.PlaySFXSound(soundReferences.GameOver);
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(GameManager.Instance.globalConfig.gameScene);
    }

    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene(GameManager.Instance.globalConfig.menuScene);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
