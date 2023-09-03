using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : Panel, IUpdate
{
    public TMP_Text txtTimer;
    public SimpleBar experienceBar;
    public string levelString = "Level {0}";
    public SimpleBar manaBar;
    public string manaString = "Mana {0}/{1}";
    public SimpleBar hpBar;
    public string hpString = "HP {0}/{1}";

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
        GameManager.Instance.experienceSystem.OnUpdateExperience += UpdateExperience;
        GameManager.Instance.experienceSystem.OnUpdateLevel += UpdateLevel;
        GameManager.Instance.manaSystem.OnUpdateMana += UpdateMana;
        GameManager.Instance.Player.LifeController.OnLifeUpdate += UpdatePlayerLife;

        experienceBar.SetValue(0);
        experienceBar.txtTitle.SetText(levelString, GameManager.Instance.experienceSystem.CurrentLevel);

        UpdateMana(GameManager.Instance.manaSystem.CurrentT, GameManager.Instance.manaSystem.currentMana, GameManager.Instance.manaSystem.maxMana);
    }

    public void Refresh()
    {
        UpdateTimer(GameManager.Instance.updateManager.CurrentTimeGameplay);
    }

    public void UpdateTimer(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        txtTimer.text = time.ToString(GameManager.Instance.globalConfig.timeFormat);
    }

    private void UpdateExperience(float amount)
    {
        experienceBar.SetValue(amount);
    }

    private void UpdateLevel(int level)
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

}
