using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : Panel
{
    [Header("Level Up")]
    [SerializeField] private PowerUpButton powerButtonPrefab;

    private List<PowerUpButton> buttons = new List<PowerUpButton>();

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < GameManager.Instance.playerData.maxPowerUpSelection; i++)
        {
            var button = Instantiate(powerButtonPrefab, powerButtonPrefab.transform.parent);
            button.Initialize();
            buttons.Add(button);
            button.txtTitle.SetText("Option {0}", i+1);
            button.Button.onClick.AddListener(() => { OnPowerButtonSelected(button); });
        }

        powerButtonPrefab.Show(false);
    }

    private void OnPowerButtonSelected(PowerUpButton button)
    {
        Debug.Log($"Selected {button.txtTitle.text}");
        Close();
    }

    public override void Open()
    {
        base.Open();

        GameManager.Instance.SetPause(true, pauseMenu: false);
        buttons[0].Button.Select();
    }

    public override void Close()
    {
        base.Close();
        GameManager.Instance.SetPause(false, pauseMenu: false);
    }

}
