using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "PUAbility_", menuName = "TP/PowerUp/PowerUpAbility", order = 1)]
public class PowerUpAbilitySO : BasePowerUpSO
{
    [Header("Ability")]
    [SerializeField] private AbilityDataSO abilityData;
    [SerializeField] private BaseAbilityAction action;

    [SerializeField] private float cooldownMultiplier;
    [SerializeField] private float manaCostMultiplier;
    [SerializeField] private float amountMultiplier;
    [SerializeField] private float attackMultiplier;

    public float AmountMultiplier => amountMultiplier;
    public float AttackMultiplier => attackMultiplier;
    public AbilityDataSO AbilityData => abilityData;

    private string description;
    private StringBuilder descriptionStringBuilder = new StringBuilder();
    private string initialDescription = "Gain a new boost for {0}";
    private string reduceEnumeration = "- Reduce {0} by {1:0}%";

    public override void Initialize()
    {
        Debug.Assert(abilityData != null, $"PowerUp {name} needs an ability");

        if (icon == null)
            icon = abilityData.Icon;

        if(amountMultiplier > 0 || attackMultiplier > 0)
            Debug.Assert(action != null, $"PowerUp {name} needs an action if it's going to multiply it's amount or attack");

        descriptionStringBuilder = new StringBuilder();
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.AppendFormat(initialDescription, abilityData.title);
        descriptionStringBuilder.AppendLine();

        if (cooldownMultiplier > 0)
        {
            descriptionStringBuilder.AppendFormat(reduceEnumeration, "Cooldown", cooldownMultiplier * 100);
            descriptionStringBuilder.AppendLine();
        }

        if (manaCostMultiplier > 0)
        {
            descriptionStringBuilder.AppendFormat(reduceEnumeration, "Mana", manaCostMultiplier * 100);
            descriptionStringBuilder.AppendLine();
        }

        if (action != null)
            action.GetDescriptionForPowerUp(descriptionStringBuilder, this);

        description = descriptionStringBuilder.ToString();
    }

    public override void Execute()
    {
        base.Execute();

        if(action != null)
            action.PowerUp(amountMultiplier, attackMultiplier);

        abilityData.Cooldown = abilityData.Cooldown - (abilityData.Cooldown * cooldownMultiplier);
        abilityData.ManaCost = abilityData.ManaCost - (int)(abilityData.ManaCost * manaCostMultiplier);
    }

    public override string GetDescription()
    {
        return description;
    }
}
