using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "TP/PlayerData", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    public bool canTakeDamage = true;
    public int maxMana = 50;
}
