using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy0_EnemyData", menuName = "TP/General/EnemyData", order = 3)]
public class EnemyDataSO : ScriptableObject
{
    public string poolName;
    public int weight;
    [Tooltip("How many can be in the scene at the same time. If 0, there is no limit")]
    public int limitAmount;
    [Tooltip("How many can appear in the same run. If 0, there is no limit")]
    public int maxLimitPerRun;

    public int Index { get; set; }
    public bool CanBeSpawned => weight > 0;
    public bool HasMaxLimitRun => maxLimitPerRun > 0;
    public bool HasLimitAmount => limitAmount > 0;
    public int Weight => weight;
}
