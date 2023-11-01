using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : Panel, IUpdate
{
    [Header("Top")]
    public TMP_Text txtTimer;
    public SimpleBar experienceBar;
    public string levelString = "Level {0}";
    public SimpleBar hpBar;
    public string hpString = "HP {0}/{1}";

    [Header("Bottom")]
    public SimpleBar manaBar;
    public string manaString = "Mana {0}/{1}";
    public AbilityUI abilityPrototype;
    public List<AbilityUI> abilityUIList = new List<AbilityUI>();

    [Header("Extra")]
    [SerializeField] public Panel gameOverPanel;

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
        GameManager.Instance.experienceSystem.OnUpdateExperience += UpdateExperience;
        GameManager.Instance.experienceSystem.OnUpdateLevel += UpdateLevel;
        GameManager.Instance.manaSystem.OnUpdateMana += UpdateMana;
        GameManager.Instance.Player.LifeController.OnLifeUpdate += UpdatePlayerLife;
        GameManager.Instance.Player.OnUnlockedAbilityEvent += UnlockAbility;

        experienceBar.SetValue(0);
        experienceBar.txtTitle.SetText(levelString, GameManager.Instance.experienceSystem.CurrentLevel);

        for (int i = 0; i < GameManager.Instance.playerData.maxAbilities; i++)
        {
            var ability = Instantiate(abilityPrototype, abilityPrototype.transform.parent);
            ability.gameObject.name = $"CNT_Ability_{i}";
            ability.Initialize();
            abilityUIList.Add(ability);
        }

        abilityPrototype.gameObject.SetActive(false);

        gameOverPanel.Initialize();
        gameOverPanel.Close();

        UpdateMana(GameManager.Instance.manaSystem.CurrentT, GameManager.Instance.manaSystem.currentMana, GameManager.Instance.manaSystem.maxMana);
    }

    public void Refresh()
    {
        UpdateTimer(GameManager.Instance.updateManager.CurrentTimeGameplay);
        UpdateAbilities();
    }

    private void UpdateAbilities()
    {
        for (int i = 0; i < abilityUIList.Count; i++)
            abilityUIList[i].Refresh();
    }

    public void UpdateTimer(float timeInSeconds)
    {
        txtTimer.SetText(TimeSpan.FromSeconds(timeInSeconds).ToString(GameManager.Instance.globalConfig.timeFormat));
    }

    private void UpdateExperience(float amount)
    {
        experienceBar.SetValue(amount);
    }

    public void UpdateLevel(int level)
    {
        experienceBar.txtTitle.SetText(levelString, level);
    }

    private void UpdateMana(float fillAmount, int currentMana, int maxMana)
    {
        manaBar.SetValue(fillAmount);
        manaBar.txtTitle.SetText(manaString, currentMana, maxMana);
    }

    private void UpdatePlayerLife(int currentLife, int maxLife)
    {
        hpBar.SetValue(Mathf.InverseLerp(0, maxLife, currentLife));
        hpBar.txtTitle.SetText(hpString, currentLife, maxLife);
    }

    private void UnlockAbility(AbilityDataSO abilityData)
    {
        AbilityUI ability = null;
        int currentIndex = 0;
        for (int i = 0; i < abilityUIList.Count; i++)
        {
            if (abilityUIList[i].IsUnlocked) continue;
            ability = abilityUIList[i];
            currentIndex = i;
            break;
        }

        if (ability == null)
        {
            Debug.LogError("Trying to assing an ability when not slots available");
            return;
        }

        ability.SetAbility(abilityData, currentIndex);
    }
}
