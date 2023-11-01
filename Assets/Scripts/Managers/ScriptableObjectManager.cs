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
    const string ATTACKDATA_PATH = "Data/Attack";

    public static ScriptableObjectManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    public Sprite defaultPowerUpIcon;

    [Header("Loadead SO")]
    [ReadOnly] public AbilityDataSO[] allAbilities = new AbilityDataSO[0];
    [ReadOnly] public BasePowerUpSO[] allPowerUps = new BasePowerUpSO[0];
    [ReadOnly] public AttackDataSO[] allAttacksData = new AttackDataSO[0];

    [Header("Info")]
    [SerializeField, ReadOnly] private bool initialized = false;
    [ReadOnly, SerializeField] private int currentPowerUps = 0;
    [field: NonSerialized, ReadOnly] public List<AbilityDataSO> AllUnlockableAbilities { get; private set; }
    [field: NonSerialized, ReadOnly] public List<BasePowerUpSO> AllPowerUps { get; private set; }



    private Dictionary<AbilityDataSO, List<BasePowerUpSO>> abilityToPowerUpList = new Dictionary<AbilityDataSO, List<BasePowerUpSO>>();
    private HashSet<BaseAbilityAction> allLoadedActions = new HashSet<BaseAbilityAction>();

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
        allAttacksData = Resources.LoadAll<AttackDataSO>(ATTACKDATA_PATH);

        for (int i = 0; i < allPowerUps.Length; i++)
        {
            if (allPowerUps[i].Icon != null) continue;
            if (allPowerUps[i] is PowerUpAbilitySO) continue;

            allPowerUps[i].SetOverrideIcon(defaultPowerUpIcon);
        }
    }

    public void Resort()
    {
        abilityToPowerUpList.Clear();

        AllUnlockableAbilities = new List<AbilityDataSO>(allAbilities);
        AllPowerUps = new List<BasePowerUpSO>();

        for (int i = 0; i < allAttacksData.Length; i++)
            allAttacksData[i].Initialize();

        for (int i = 0; i < AllUnlockableAbilities.Count; i++)
        {
            var newList = new List<BasePowerUpSO>();

            AllUnlockableAbilities[i].Initialize();

            for (int j = 0; j < AllUnlockableAbilities[i].actions.Length; j++)
            {
                if (allLoadedActions.Contains(AllUnlockableAbilities[i].actions[j]))
                {
                    Debug.LogError($"{AllUnlockableAbilities[i].name} has the ability {AllUnlockableAbilities[i].actions[j].name} repeteated somewhere");
                    continue;
                }

                allLoadedActions.Add(AllUnlockableAbilities[i].actions[j]);
            }

            abilityToPowerUpList.Add(AllUnlockableAbilities[i], newList);
        }

        SortOutPowerUps();
    }

    private void SortOutPowerUps()
    {
        for (int i = 0; i < allPowerUps.Length; i++)
        {
            allPowerUps[i].Initialize();

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

        currentPowerUps = AllPowerUps.Count;
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

        currentPowerUps = AllPowerUps.Count;
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

    public void RemoveUsedPowerUp(BasePowerUpSO basePowerUp)
    {
        if (basePowerUp.CanBeReutilized()) return;

        if (!AllPowerUps.Contains(basePowerUp))
        {
            Debug.LogError($"Trying to remove a power up that is not in the list {basePowerUp.name}");
            return;
        }

        AllPowerUps.Remove(basePowerUp);
        currentPowerUps = AllPowerUps.Count;
    }

    public bool HasPowerUpsToUnlock(int minimumNeeded)
    {
        return AllPowerUps.Count >= minimumNeeded;
    }
}
