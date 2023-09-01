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

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
        GameManager.Instance.experienceSystem.OnUpdateExperience += UpdateExperience;
        GameManager.Instance.experienceSystem.OnUpdateLevel += UpdateLevel;

        experienceBar.SetValue(0);
        experienceBar.txtTitle.SetText(levelString, GameManager.Instance.experienceSystem.CurrentLevel);
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

}
