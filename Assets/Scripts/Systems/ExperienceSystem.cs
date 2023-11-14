using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class ExperienceSystem : MonoBehaviour
{
    [ReadOnly] private float currentXP;
    [ReadOnly] private float requiredXP;
    [ReadOnly] private int currentLevel = 0;
    [ReadOnly] private float experienceMultiplier = 1;

    public float ExperienceMultiplier => experienceMultiplier;
    public float CurrentT { get; private set; }
    public float CurrentLevel => currentLevel;
    public int AmountLeveledUp { get; private set; }

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
        requiredXP = GetNextLevelExp();
        currentXP = 0;
        RecalculateCurrentT();
    }

    public void AddExperience(float xp)
    {
        if (GameManager.Instance.globalConfig.HasLevelCap && currentLevel > GameManager.Instance.globalConfig.maxLevelCap) return;
        currentXP += (xp * experienceMultiplier);

        if (currentXP >= requiredXP)
            LevelUp();

        RecalculateCurrentT();
    }

    public void LevelUp()
    {
        AmountLeveledUp = 0;

        while (currentXP >= requiredXP)
        {
            AmountLeveledUp++;

            if (GameManager.Instance.globalConfig.CanScaleDifficult(currentLevel + AmountLeveledUp))
                ScaleUpDifficulty();

            currentXP = Mathf.Abs(requiredXP - currentXP);
            requiredXP = GetNextLevelExp();
        }

        currentXP = Mathf.Clamp(currentXP, 0, requiredXP);

        currentLevel += AmountLeveledUp;

        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.levelUpSound);
        GameManager.Instance.gameplayUIManager.LevelUp(currentLevel);
    }

    private void ScaleUpDifficulty()
    {
        //print("Difficulty scaled");
        //GameManager.Instance.globalConfig.ScaleDifficulty();
    }

    private float GetNextLevelExp()
    {
        var experienceModifier = GameManager.Instance.playerData.experiencesModifier;
        return currentLevel / experienceModifier + currentLevel % experienceModifier *  100f * Mathf.Pow(experienceModifier, currentLevel / experienceModifier);
    }

    private void RecalculateCurrentT()
    {
        CurrentT = Mathf.InverseLerp(0, requiredXP, currentXP);
        OnUpdateExperience?.Invoke(CurrentT);
    }

    public void SetNexExperienceMultiplier(float nexExperienceMultiplier)
    {
        if(nexExperienceMultiplier <= 0)
        {
            Debug.LogError("Trying to set a new experience multiplier that it's too low");
            return;
        }

        experienceMultiplier = nexExperienceMultiplier;
    }

    public float GetExpNeedForNextLeve()
    {
        return requiredXP - currentXP;
    }
}
