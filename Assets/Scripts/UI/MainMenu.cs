using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    public GlobalConfigSO globalConfig;
    public SoundReferencesSO soundReferences;
    public PrefabsReferencesSO prefabsReferences;
    public Panel menu;
    public Panel credits;

    [Header("Buttons")]
    public MenuButton playButton;
    public MenuButton creditsButton;
    public MenuButton quitButton;
    public MenuButton goBackButton;

    [Header("Info")]
    [ReadOnly] public AudioManager audioManager;

    private MenuButton currentSelectedButton;
    private List<MenuButton> buttons = new List<MenuButton>();
    private static GameInputs _inputs;

    void Awake()
    {
        Instantiate(prefabsReferences.eventSystemPrefab);
        audioManager = Instantiate(prefabsReferences.audioManagerPrefab);

        buttons.Add(playButton);
        buttons.Add(creditsButton);
        buttons.Add(quitButton);
        buttons.Add(goBackButton);

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Initialize();
            //buttons[i].Button.onClick.AddListener(SelectButtonSound);
            buttons[i].Deselect();
        }

        playButton.Button.onClick.AddListener(OnClickPlayHandler);
        creditsButton.Button.onClick.AddListener(OnClickCreditsHandler);
        quitButton.Button.onClick.AddListener(OnClickQuitHandler);
        goBackButton.Button.onClick.AddListener(OnClickGoBackHandler);

        _inputs = new GameInputs();
        _inputs.Enable();
        _inputs.Menu.GoBack.performed += GoBack;
    }

    private void Start()
    {
        //audioManager.PlayMusic(soundReferences.mainMenu);
        currentSelectedButton = playButton;
        GoBack();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Button.onClick.RemoveAllListeners();

        _inputs.Menu.GoBack.performed -= GoBack;
    }

    private void SkipMenu(InputAction.CallbackContext cxt)
    {
        OnClickPlayHandler();
    }

    private void GoBack(InputAction.CallbackContext cxt)
    {
        if (!credits.IsOpen) return;
        GoBack();
    }

    private void GoBack()
    {
        menu.Open();
        credits.Close();
        SelectButton(currentSelectedButton);
    }

    private void SelectButtonSound()
    {
        //audioManager.PlaySFXSound(soundReferences.selectButton);
    }

    private void OnClickPlayHandler()
    {
        SceneManager.LoadScene(globalConfig.gameScene);
    }

    private void OnClickCreditsHandler()
    {
        currentSelectedButton = creditsButton;
        menu.Close();
        credits.Open();
        SelectButton(goBackButton);
    }

    private void SelectButton(MenuButton button)
    {
        button.Button.Select();
    }

    private void OnClickGoBackHandler()
    {
        GoBack();
    }

    private void OnClickQuitHandler()
    {
        Application.Quit();
    }
}
