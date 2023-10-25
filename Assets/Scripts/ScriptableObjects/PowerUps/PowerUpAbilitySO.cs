using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PUStat_", menuName = "TP/PowerUp/PowerUpStat", order = 0)]
public class PowerUpAbilitySO : BasePowerUpSO
{
    [Header("Ability")]
    [SerializeField] private AbilityDataSO abilityData;
    [SerializeField] private BaseAbilityAction action;

    [SerializeField] private float cooldownMultiplier;
    [SerializeField] private float manaCostMultiplier;
    [SerializeField] private float amountMultiplier;
    [SerializeField] private float attackMultiplier;

    public AbilityDataSO AbilityData => abilityData;

    public override void Initialize()
    {
        newDescription = description;

        newDescription += "<br>";

        if (cooldownMultiplier > 0)
            description += $"- Reduce Cooldown: x{cooldownMultiplier}";
        if (manaCostMultiplier > 0)
            description += $"- Reduce Mana: x{manaCostMultiplier}";
        if(amountMultiplier > 0)
            description += $"- Amount Multiplier: x{amountMultiplier}";
        if(attackMultiplier > 0)
            description += $"- Attack Multiplier: x{attackMultiplier}";
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        return description;
    }
}
