using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    const string ABILITIES_PATH = "Data/Abilities/";
    const string POWERUP_PATH = "Data/PowerUps/";

    public static ScriptableObjectManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    [Header("Loadead SO")]
    [ReadOnly] public AbilityDataSO[] allAbilities = new AbilityDataSO[0];
    [ReadOnly] public BasePowerUpSO[] allPowerUps = new BasePowerUpSO[0];
    [field: NonSerialized, ReadOnly] public List<AbilityDataSO> AllUnlockableAbilities { get; private set; }
    [field: NonSerialized, ReadOnly] public List<BasePowerUpSO> AllPowerUps { get; private set; }

    [Header("Extra Info")]
    [SerializeField, ReadOnly] private bool initialized = false;

    private Dictionary<AbilityDataSO, List<BasePowerUpSO>> abilityToPowerUpList = new Dictionary<AbilityDataSO, List<BasePowerUpSO>>();

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    public void Initialize()
    {
        if (initialized) return;
        initialized = true;

        allAbilities = Resources.LoadAll<AbilityDataSO>(ABILITIES_PATH);
        allPowerUps = Resources.LoadAll<BasePowerUpSO>(POWERUP_PATH);

        abilityToPowerUpList.Clear();

        AllUnlockableAbilities = new List<AbilityDataSO>(allAbilities);

        for (int i = 0; i < AllUnlockableAbilities.Count; i++)
        {
            var newList = new List<BasePowerUpSO>();
            abilityToPowerUpList.Add(AllUnlockableAbilities[i], newList);
        }

        SortOutPowerUps();
    }

    public void SortOutPowerUps()
    {
        for (int i = 0; i < allPowerUps.Length; i++)
        {
            if (allPowerUps[i] is PowerUpPlayerStatsSO)
            {
                AllPowerUps.Add(allPowerUps[i]);
                continue;
            }

            var abilityPowerUp = allPowerUps[i] as PowerUpAbilitySO;


            if (abilityPowerUp.AbilityData == null)
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

    public void RemoveUnlockedAbility(AbilityDataSO unlockedAbility)
    {
        if(!AllUnlockableAbilities.Contains(unlockedAbility))
        {
            Debug.LogError($"Trying to remove an ability that is not there {unlockedAbility.name}");
            return;
        }

        AllUnlockableAbilities.Remove(unlockedAbility);
        AddToPowerUpUnlockables(unlockedAbility);
    }
}
