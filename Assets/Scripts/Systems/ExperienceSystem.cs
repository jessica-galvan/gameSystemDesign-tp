using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    [ReadOnly] private float currentXP;
    [ReadOnly] private float requiredXP;
    [ReadOnly] private int currentLevel = 0;

    public float CurrentT { get; private set; }
    public float CurrentLevel => currentLevel;

    /// <summary>
    /// Will give you the current fillAmount
    /// </summary>
    public Action<float> OnUpdateExperience;

    /// <summary>
    /// Will give you the current level
    /// </summary>
    public Action<int> OnUpdateLevel;

    public void Initialize()
    {
        currentLevel = 1;
        requiredXP = 100; //TODO get required XP from curve or config
        currentXP = 0;
        RecalculateCurrentT();
    }

    public void AddExperience(float xp)
    {
        currentXP += xp;

        if (currentXP >= requiredXP)
            LevelUp();

        RecalculateCurrentT();
    }

    public void LevelUp()
    {
        currentLevel++;
        currentXP = requiredXP - currentXP;
        //TODO requiredXP if has a curve, should be recalculate here;
        currentXP = Mathf.Clamp(currentXP, 0, requiredXP);

        GameManager.Instance.gameplayUIManager.LevelUp(currentLevel);
        //OnUpdateLevel?.Invoke(currentLevel);
    }

    private void RecalculateCurrentT()
    {
        CurrentT = Mathf.InverseLerp(0, requiredXP, currentXP);
        OnUpdateExperience?.Invoke(CurrentT);
    }

}
