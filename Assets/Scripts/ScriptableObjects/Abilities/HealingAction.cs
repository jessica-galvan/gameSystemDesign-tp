using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingAction", menuName = "TP/AbilityData/HealingAction", order = 3)]
public class HealingAction : BaseAbilityAction
{
    public int baseHealthingPercentage = 0;

    [field: ReadOnly, NonSerialized] public int HealingPercentage { get; set; }

    public override void Initialize()
    {
        HealingPercentage = baseHealthingPercentage;
    }

    public override void Execute(PlayerModel playerModel)
    {
        playerModel.LifeController.Heal(HealingPercentage);

        Debug.Log($"Ability {name} was executed");
    }

    public override void PowerUp(float amountMultiplier, float attackMultiplier)
    {
        if(amountMultiplier > 0)
            HealingPercentage += Mathf.RoundToInt(HealingPercentage * amountMultiplier);

        Debug.Assert(attackMultiplier <= 0, $"Trying to add attack multiplier to a healing ability {name}");
    }

    public override void GetDescriptionForPowerUp(StringBuilder stringBuilder, PowerUpAbilitySO powerUp)
    {
        stringBuilder.AppendLine($"- Gains an {powerUp.AmountMultiplier * 10}% of extra healing");
    }
}