using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUpPanel : Panel
{
    [Header("Level Up")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PowerUpButton powerButtonPrefab;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Info")]
    [SerializeField, ReadOnly] private bool optionSelected = false, isPowerUpSelection = false;

    private int currentAmountSelected = 0;
    private List<PowerUpButton> buttons = new List<PowerUpButton>();
    private HashSet<AbilityDataSO> currentAbilities = new HashSet<AbilityDataSO>();
    private HashSet<BasePowerUpSO> currentPowerUps = new HashSet<BasePowerUpSO>();

    private HashSet<AbilityDataSO> unlockedAbilities = new HashSet<AbilityDataSO>();
    private HashSet<BasePowerUpSO> unlockedPowerUps = new HashSet<BasePowerUpSO>();

    public override void Initialize()
    {
        base.Initialize();

        var maxSelection = Mathf.Max(GameManager.Instance.playerData.maxPowerUpSelection, GameManager.Instance.playerData.maxAbilitySelection);

        for (int i = 0; i < maxSelection; i++)
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

        currentAmountSelected++;

        Debug.Log($"Selected {button.CurrentOption.Title}");

        if (button.CurrentOption is AbilityDataSO)
        {
            var ability = button.CurrentOption as AbilityDataSO;
            GameManager.Instance.Player.UnlockAbility(ability);
            ScriptableObjectManager.Instance.RemoveUnlockedAbility(ability);
        }

        if(button.CurrentOption is BasePowerUpSO)
        {
            var powerUp = button.CurrentOption as BasePowerUpSO;
            powerUp.Execute();
            ScriptableObjectManager.Instance.RemoveUsedPowerUp(powerUp);
        }

        if (currentAmountSelected >= GameManager.Instance.experienceSystem.AmountLeveledUp)
            Close();
        else
            SetUpSelection();

    }

    public override void Open()
    {
        canvasGroup.alpha = 0;
        base.Open();

        currentAmountSelected = 0;
        GameManager.Instance.SetPause(true, pauseMenu: false);
        GameManager.Instance.UIEffects.SetLevelUpEffects(true);

        SetUpSelection();

        StartCoroutine(LevelupCoroutine());
    }

    private void SetUpSelection()
    {
        currentAbilities.Clear();
        currentPowerUps.Clear();

        var currentLevel = GameManager.Instance.experienceSystem.CurrentLevel - (GameManager.Instance.experienceSystem.AmountLeveledUp + currentAmountSelected);
        isPowerUpSelection = !IsAbilitySelection(currentLevel);

        SetOptions();
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
        var selectionAmount = isPowerUpSelection ? GameManager.Instance.playerData.maxPowerUpSelection : GameManager.Instance.playerData.maxAbilitySelection;

        for (int i = 0; i < selectionAmount; i++)
        {
            ISelectableOption selection = null;
            if(isPowerUpSelection)
                selection = GetPowerUpRandomSelection();
            else
                selection = GetAbilityRandomSelection();

            buttons[i].Show(true);
            buttons[i].SetSelectableOption(selection);
        }

        for (int i = selectionAmount; i < buttons.Count; i++)
            buttons[i].Show(false);
    }

    public void SetOptionsEnabled(bool isEnabled)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Button.enabled = isEnabled;
    }

    public ISelectableOption GetAbilityRandomSelection()
    {
        var ability = RandomWeight<AbilityDataSO>.Run(ScriptableObjectManager.Instance.AllUnlockableAbilities, out var index);

        if (!currentAbilities.Contains(ability))
            currentAbilities.Add(ability);
        else
            return GetAbilityRandomSelection();

        return ability;
    }

    public ISelectableOption GetPowerUpRandomSelection()
    {
        var powerup = RandomWeight<BasePowerUpSO>.Run(ScriptableObjectManager.Instance.AllPowerUps, out var index);

        if (!currentPowerUps.Contains(powerup))
            currentPowerUps.Add(powerup);
        else
            return GetPowerUpRandomSelection();

        return powerup;
    }

    public bool IsAbilitySelection(float currentLevel)
    {
        return currentLevel % 3 == 0;
    }
}
