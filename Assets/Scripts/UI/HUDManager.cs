using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : Panel, IUpdate
{
    public TMP_Text txtTimer;

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
    }

    public void Refresh()
    {
        UpdateTimer(GameManager.Instance.updateManager.CurrentTimeGameplay);
    }

    public void UpdateTimer(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        txtTimer.text = time.ToString("mm':'ss");
    }

}
