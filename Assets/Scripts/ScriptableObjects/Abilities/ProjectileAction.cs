using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAction", menuName = "TP/AbilityData/ProjectileAction", order = 1)]
public class ProjectileAction : BaseAbilityAction
{
    public ProjectileController projectilePrefab;
    public int baseAmount = 0;
    public float radiousFromPlayer = 0f;
    public Vector2[] directions;

    [field: ReadOnly, NonSerialized] public float Amount { get; set; }

    public override void Initialize()
    {
        Amount = baseAmount;
    }

    public override void Execute(BaseCharacterModel characterModel)
    {
        Debug.Log($"Ability {name} was executed");
    }
}

