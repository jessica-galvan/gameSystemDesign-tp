using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiateAction", menuName = "TP/AbilityData/InstantiateAction", order = 2)]
public class InstantiateAction : BaseAbilityAction
{
    public GameObject objectToInstantiate;
    public int baseAmount = 0;
    public float minDistanteFromPlayer = 0f;
    public float maxDistanteFromPlayer = 5f;
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
