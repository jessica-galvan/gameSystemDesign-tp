using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "TP/General/PlayerData", order = 2)]
public class PlayerDataSO : ScriptableObject
{
    public int maxAbilities = 3;
    public int maxMana = 50;
    public int maxPowerUpSelection = 3;
    public bool canCheat = false;

    [SerializeField] private List<AbilityDataSO> allUnlockableAbilities = new List<AbilityDataSO>();

    [field: NonSerialized, ReadOnly] public List<AbilityDataSO> AllUnlockableAbilities { get; private set; }

    public void Initialize()
    { 
        AllUnlockableAbilities = new List<AbilityDataSO>(allUnlockableAbilities);
    }
}
