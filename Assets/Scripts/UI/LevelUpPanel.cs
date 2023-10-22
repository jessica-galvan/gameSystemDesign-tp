using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : Panel
{
    [Header("Level Up")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PowerUpButton powerButtonPrefab;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float fadeDuration = 2f;

    private bool optionSelected = false;

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
        if (optionSelected) return;
        optionSelected = true;

        Debug.Log($"Selected {button.CurrentOption.Title}");

        if (button.CurrentOption is AbilityDataSO)
        {
            print("Ability was unlocked");
            GameManager.Instance.Player.UnlockAbility(button.CurrentOption as AbilityDataSO);
        }

        Close();
    }

    public override void Open()
    {
        canvasGroup.alpha = 0;
        base.Open();

        GameManager.Instance.SetPause(true, pauseMenu: false);
        GameManager.Instance.UIEffects.SetLevelUpEffects(true);
        SetOptions();
        StartCoroutine(LevelupCoroutine());
    }

    private IEnumerator LevelupCoroutine()
    {
        yield return new WaitForSecondsRealtime(waitTime);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeDuration;
            canvasGroup.alpha = t;
            yield return null;
        }

        SetOptionsEnabled(true);
        buttons[0].Button.Select();
    }

    public override void Close()
    {
        base.Close();

        optionSelected = false;
        SetOptionsEnabled(false);

        GameManager.Instance.UIEffects.SetLevelUpEffects(false);
        GameManager.Instance.SetPause(false, pauseMenu: false);
    }

    public void SetOptions()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var selection = GetRandomSelection(i);
            buttons[i].SetSelectableOption(selection);
        }
    }

    public void SetOptionsEnabled(bool isEnabled)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Button.enabled = isEnabled;
    }

    public ISelectableOption GetRandomSelection(int i)
    {
        //TODO implent a system with random weight for this shit
        //and make sure it only shows abilities that haven't been unlocked
        return GameManager.Instance.playerData.allUnlockableAbilities[i];
    }

}
