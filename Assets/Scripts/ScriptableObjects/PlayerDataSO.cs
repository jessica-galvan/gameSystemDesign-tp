using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "TP/PlayerData", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    public int maxMana = 50;
    public int maxPowerUpSelection = 3;
}
