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
    public bool canCheat = false;

    [SerializeField] private List<AbilityDataSO> allUnlockableAbilities = new List<AbilityDataSO>();
    [SerializeField] private List<BasePowerUpSO> allPowerUps = new List<BasePowerUpSO>();

    [field: NonSerialized, ReadOnly] public List<AbilityDataSO> AllUnlockableAbilities { get; private set; }
    [field: NonSerialized, ReadOnly] public List<BasePowerUpSO> AllPowerUps { get; private set; }

    private Dictionary<AbilityDataSO, List<BasePowerUpSO>> abilityToPowerUpList = new Dictionary<AbilityDataSO, List<BasePowerUpSO>>();

    public void Initialize()
    {
        abilityToPowerUpList.Clear();

        AllUnlockableAbilities = new List<AbilityDataSO>(allUnlockableAbilities);

        for (int i = 0; i < AllUnlockableAbilities.Count; i++)
        {
            var newList = new List<BasePowerUpSO>();
            abilityToPowerUpList.Add(AllUnlockableAbilities[i], newList);
        }

        SortOutPowerUps();
    }

    public void SortOutPowerUps()
    {
        for (int i = 0; i < allPowerUps.Count; i++)
        {
            if (allPowerUps[i] is PowerUpPlayerStatsSO)
            {
                AllPowerUps.Add(allPowerUps[i]);
                continue;
            }

            var abilityPowerUp = allPowerUps[i] as PowerUpAbilitySO;


            if(abilityPowerUp.AbilityData == null)
            {
                Debug.LogError(abilityPowerUp.name + " has no ability asigned");
                continue;
            }


            if (!abilityToPowerUpList.TryGetValue(abilityPowerUp.AbilityData, out var powerUpList)) continue;
            powerUpList.Add(abilityPowerUp);
        }
    }

    public void AddToPowerUpUnlockables(AbilityDataSO abilityDataSO)
    {
        if (!abilityToPowerUpList.TryGetValue(abilityDataSO, out var powerUpList))
        {
            Debug.LogError(abilityDataSO.name + " is not in the dictionary");
            return;
        }

        for (int i = 0; i < powerUpList.Count; i++)
        {
            if (AllPowerUps.Contains(powerUpList[i])) continue;
            AllPowerUps.Add(allPowerUps[i]);
        }
    }
}
