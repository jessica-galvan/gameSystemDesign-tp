using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "TP/General/PlayerData", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    public int maxAbilities = 3;
    public int maxMana = 50;
    public int maxPowerUpSelection = 3;
    public int maxAbilitySelection = 2;
    public bool canCheat = false;
}
