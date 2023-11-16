using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class ExperienceSystem : MonoBehaviour
{
    [ReadOnly, SerializeField] private float currentXP;
    [ReadOnly, SerializeField] private float requiredXP;
    [ReadOnly, SerializeField] private int currentLevel = 0;
    [ReadOnly, SerializeField] private float experienceMultiplier = 1;
    [ReadOnly, SerializeField] private float totalExperienceGained = 0;
    [ReadOnly, SerializeField] private int killedEnemiesThisLevel = 0;

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
        requiredXP = GameManager.Instance.playerData.startingRequiredExp;
        currentXP = 0;
        RecalculateCurrentT();
    }

    public void AddExperience(float xp, bool withBonusMultiplier = true)
    {
        if (GameManager.Instance.globalConfig.HasLevelCap && currentLevel > GameManager.Instance.globalConfig.maxLevelCap) return;

        killedEnemiesThisLevel++;

        if (withBonusMultiplier)
            xp += (xp * experienceMultiplier);
        
        currentXP += xp;
        totalExperienceGained += xp;

        if (currentXP >= requiredXP)
            LevelUp();

        RecalculateCurrentT();
    }

    public void LevelUp()
    {
        AmountLeveledUp = 0;

        while (currentXP >= requiredXP)
        {
            currentLevel++;
            AmountLeveledUp++;

            if (GameManager.Instance.globalConfig.CanScaleDifficult(currentLevel))
                ScaleUpDifficulty();

            currentXP = Mathf.Abs(requiredXP - currentXP);
            requiredXP = GetNextLevelExp();
            print($"Required XP for level {currentLevel}: {requiredXP}. Killed Enemies: {killedEnemiesThisLevel}");
        }

        currentXP = Mathf.Clamp(currentXP, 0, requiredXP);

        killedEnemiesThisLevel = 0;

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
        return Mathf.Round(requiredXP + Mathf.Abs((requiredXP * currentLevel * Mathf.Log(GameManager.Instance.playerData.experienceGrowth, currentLevel))));
        //return Mathf.Sqrt(experienceModifier * (power * totalExperienceGained + 25) + 50) / 100;
        //return (currentLevel ^ power + currentLevel) / power * experienceModifier - (currentLevel * experienceModifier); 
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
