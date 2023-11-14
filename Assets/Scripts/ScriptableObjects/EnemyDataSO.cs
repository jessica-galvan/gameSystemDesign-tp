using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy0_EnemyData", menuName = "TP/General/EnemyData", order = 3)]
public class EnemyDataSO : ScriptableObject
{
    public string poolName;
    public int id;

    public int ID => id;
}
