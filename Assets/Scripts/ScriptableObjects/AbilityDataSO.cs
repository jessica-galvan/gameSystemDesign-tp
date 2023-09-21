using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityDataSO : ScriptableObject
{
    [Header("Basic info")]
    public string title = "TITLE";
    public Sprite icon;
    [TextArea] public string description = "Add a description here";
    public float cooldown = 0.5f;
    public int manaCost = 5;

    //[Header("Movement")]
    //public bool locksMovement = false;
    //public bool locksFacing = false;
    //public float movementCooldown = 0f;

    [field: ReadOnly, NonSerialized] public int ManaCost { get; set; }
    [field: ReadOnly, NonSerialized] public float Cooldown { get; set; }

    public virtual void Initialize()
    {
        ManaCost = manaCost;
        Cooldown = cooldown;
    }

    public abstract void Execute();
}

[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "TP/AbilityData/ProjectileAbility", order = 1)]
public class ProjectileAbilityDataSO : AbilityDataSO
{
    [Header("Projectile")]
    public ProjectileController projectilePrefab;
    public int baseAmount = 0;
    public float radiousFromPlayer = 0f;
    public Vector2[] directions;

    [field: ReadOnly, NonSerialized] public float Amount { get; set; }

    public override void Initialize()
    {
        base.Initialize();

        Amount = baseAmount;
    }

    public override void Execute()
    {

    }
}

[CreateAssetMenu(fileName = "InstantiateAbility", menuName = "TP/AbilityData/InstantiateAbility", order = 2)]
public class InstantiateAbilityDataSO : AbilityDataSO
{
    [Header("Instantiate")]
    public GameObject objectToInstantiate;
    public int baseAmount = 0;
    public float minDistanteFromPlayer = 0f;
    public float maxDistanteFromPlayer = 5f;
    public Vector2[] directions;

    [field: ReadOnly, NonSerialized] public float Amount { get; set; }

    public override void Initialize()
    {
        base.Initialize();

        Amount = baseAmount;
    }

    public override void Execute()
    {

    }
}

[CreateAssetMenu(fileName = "HealingAbility", menuName = "TP/AbilityData/HealingAbility", order = 3)]
public class HealingAbilityDataSO : AbilityDataSO
{
    [Header("Health")]
    public int baseHealthingAmount = 0;

    [field: ReadOnly, NonSerialized] public float HealingAmount { get; set; }

    public override void Initialize()
    {
        base.Initialize();

        HealingAmount = baseHealthingAmount;
    }

    public override void Execute()
    {

    }
}
