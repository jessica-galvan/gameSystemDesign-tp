using System;
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
    private HashSet<AbilityDataSO> currentAbilities = new HashSet<AbilityDataSO>();

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
            var ability = button.CurrentOption as AbilityDataSO;
            GameManager.Instance.Player.UnlockAbility(ability);
            GameManager.Instance.playerData.AllUnlockableAbilities.Remove(ability);
        }

        Close();
    }

    public override void Open()
    {
        canvasGroup.alpha = 0;
        base.Open();

        currentAbilities.Clear();
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
            var selection = GetAbilityRandomSelection();
            buttons[i].SetSelectableOption(selection);
        }
    }

    public void SetOptionsEnabled(bool isEnabled)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Button.enabled = isEnabled;
    }

    public ISelectableOption GetAbilityRandomSelection()
    {
        var ability = RandomWeight<AbilityDataSO>.Run(GameManager.Instance.playerData.AllUnlockableAbilities, out var index);

        if (!currentAbilities.Contains(ability))
            currentAbilities.Add(ability);
        else
            return GetAbilityRandomSelection();

        return ability;
    }
}
