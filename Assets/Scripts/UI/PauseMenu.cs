using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : Panel
{
    [Header("Buttons")]
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private MenuButton resumeButton;
    [SerializeField] private MenuButton restartButton;
    [SerializeField] private MenuButton menuButton;
    [SerializeField] private MenuButton quitButton;

    [Header("Popup")]
    [SerializeField] private Popup warningPopup;

    private List<MenuButton> buttons = new List<MenuButton>();
    private MenuButton currentButton;

    public override void Initialize()
    {
        base.Initialize();

        buttons.Add(resumeButton);
        resumeButton.Button.onClick.AddListener(OnClickResumeHandler);

        buttons.Add(restartButton);
        restartButton.Button.onClick.AddListener(OpenRestartPopup);

        buttons.Add(menuButton);
        menuButton.Button.onClick.AddListener(OpenMainMenuPopup);
        menuButton.enabled = false;

        buttons.Add(quitButton);
        quitButton.Button.onClick.AddListener(OpenExitPopup);

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Button.onClick.AddListener(OnClick);
            buttons[i].Deselect();
        }

        currentButton = resumeButton;

        warningPopup.Close();
        warningPopup.Initialize();
        warningPopup.OnClose += SetSelectedButton;
        warningPopup.OnOpen += OnPopupOpen;
    }

    private void OnPopupOpen()
    {
        buttonsContainer.SetActive(true);
    }

    private void OnClick()
    {
        //GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.selectButton);
    }

    public override void Dispose()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Button.onClick.RemoveAllListeners();
        }

        if (warningPopup != null)
        {
            warningPopup.OnClose -= SetSelectedButton;

            if (warningPopup.IsOpen)
                warningPopup.Close();
        }
    }

    public override void Open()
    {
        base.Open();
        currentButton = resumeButton;
        SetSelectedButton();
    }

    private void PauseInput(InputAction.CallbackContext cxt)
    {
        if (warningPopup.IsOpen)
        {
            warningPopup.Close();
            SetSelectedButton();
        }
        else
        {
            GameManager.Instance.SetPause(false);
        }
    }

    private void SetSelectedButton()
    {
        buttonsContainer.SetActive(true);
        currentButton.Button.Select();
    }

    private void OpenRestartPopup()
    {
        warningPopup.SetPopupInfo(GameManager.Instance.globalConfig.restartPopup);
        warningPopup.btnConfirm.Button.onClick.AddListener(OnClickRestartHandler);
        currentButton = restartButton;
        warningPopup.Open();
        buttonsContainer.SetActive(false);
    }

    private void OpenMainMenuPopup()
    {
        warningPopup.SetPopupInfo(GameManager.Instance.globalConfig.mainMenuPopup);
        warningPopup.btnConfirm.Button.onClick.AddListener(OnClickMenuHandler);
        currentButton = menuButton;
        warningPopup.Open();
        buttonsContainer.SetActive(false);
    }

    private void OpenExitPopup()
    {
        warningPopup.SetPopupInfo(GameManager.Instance.globalConfig.exitPopup);
        warningPopup.btnConfirm.Button.onClick.AddListener(OnClickQuitHandler);
        currentButton = quitButton;
        warningPopup.Open();
        buttonsContainer.SetActive(false);
    }

    private void OnClickResumeHandler()
    {
        GameManager.Instance.SetPause(false);
    }

    private void OnClickRestartHandler()
    {
        warningPopup.Close();
        SceneManager.LoadScene(GameManager.Instance.globalConfig.gameScene);
    }

    private void OnClickMenuHandler()
    {
        warningPopup.Close();
        SceneManager.LoadScene(GameManager.Instance.globalConfig.menuScene);
    }

    private void OnClickQuitHandler()
    {
        warningPopup.Close();
        Application.Quit();
    }
}
