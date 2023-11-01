using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingAction", menuName = "TP/AbilityData/HealingAction", order = 3)]
public class HealingAction : BaseAbilityAction
{
    public int baseHealthingAmount = 0;

    [field: ReadOnly, NonSerialized] public int HealingAmount { get; set; }

    public override void Initialize()
    {
        HealingAmount = baseHealthingAmount;
    }

    public override void Execute(PlayerModel playerModel)
    {
        playerModel.LifeController.Heal(HealingAmount);

        Debug.Log($"Ability {name} was executed");
    }

    public override void PowerUp(float amountMultiplier, float attackMultiplier)
    {
        if(amountMultiplier > 0)
            HealingAmount += Mathf.RoundToInt(HealingAmount * amountMultiplier);

        Debug.Assert(attackMultiplier <= 0, $"Trying to add attack multiplier to a healing ability {name}");
    }

    public override void GetDescriptionForPowerUp(StringBuilder stringBuilder, PowerUpAbilitySO powerUp)
    {
        stringBuilder.AppendLine($"- Gains an {powerUp.AmountMultiplier * 10}% of extra healing");
    }
}