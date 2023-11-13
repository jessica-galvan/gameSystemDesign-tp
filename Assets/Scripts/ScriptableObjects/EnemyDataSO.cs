using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy0_EnemyData", menuName = "TP/General/EnemyData", order = 3)]
public class EnemyDataSO : ScriptableObject
{
    public string poolName;
    [SerializeField] private int weight;
    [Tooltip("How many can be in the scene at the same time. If 0, there is no limit")]
    [SerializeField] private int limitAmount;
    [Tooltip("Minimum Level they can start appearing to the player. Default is 0")]
    [SerializeField] private int minLevel = 0;
    [Tooltip("Maximum Level they stop appearing to the player. If -1, it has not limit")]
    [SerializeField] private int maxLevel = -1;
    [Tooltip("How many can appear in the same run. If 0, there is no limit")]
    [SerializeField] private int maxLimitPerRun;

    [Header("Difficulty Scalers")]
    [Tooltip("If you want the enemy to start appearing more, change the weight. can be used to increase or decrease. Default: -1 so it doesn't multiply")]
    [SerializeField] private float weightMultiplier = -1;
    [Tooltip("Same as Weight Multiplier")]
    [SerializeField] private float limitAmountMultiplierWithScaleDifficulty = -1;
    [Tooltip("Same as Weight Multiplier")]
    [SerializeField] private float healthMultiplier = -1;
    [Tooltip("Same as Weight Multiplier")]
    [SerializeField] private float speedMultiplier = -1;
    [Tooltip("Same as Weight Multiplier")]
    [SerializeField] private float experienceMultiplier = -1;

    public int Index { get; set; }
    public bool CanBeSpawned => weight > 0;
    public bool HasMaxLimitRun => maxLimitPerRun > 0;
    public bool HasLimitAmount => LimitAmount > 0;
    public int Weight { get; private set; }
    public int MaxLimitPerRun => maxLimitPerRun;
    public int LimitAmount { get; private set; }
    public int MaxLevel => maxLevel;
    public int MinLevel => minLevel;

    public void Initialize()
    {
        LimitAmount = limitAmount;
        Weight = weight;
    }

    public void ScaleUpDifficulty(CharacterDataSO data)
    {
        if(weightMultiplier > 0)
            Weight = Mathf.RoundToInt(Weight + Weight * weightMultiplier);

        if (HasLimitAmount && limitAmountMultiplierWithScaleDifficulty > 0)
            LimitAmount = Mathf.RoundToInt(LimitAmount + LimitAmount * limitAmountMultiplierWithScaleDifficulty);

        if(healthMultiplier > 0) 
            data.ChangeMaxLife(Mathf.RoundToInt(data.MaxLife + data.MaxLife * healthMultiplier));

        if (speedMultiplier > 0)
            data.ChangeMaxLife(Mathf.RoundToInt(data.MovementSpeed + data.MovementSpeed * speedMultiplier));

        if (experienceMultiplier > 0)
            data.ChangeMaxLife(Mathf.RoundToInt(data.Experience + data.Experience * experienceMultiplier));
    }
}
