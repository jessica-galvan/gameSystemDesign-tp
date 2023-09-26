using System;
using System.Collections;
using System.Collections.Generic;
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
}