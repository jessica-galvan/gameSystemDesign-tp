using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "TP/AbilityData/AbilityData", order = 3)]
public class AbilityDataSO : ScriptableObject, ISelectableOption, IWeight
{
    [Header("Basic info")]
    public string title = "TITLE";
    public Sprite icon;
    [TextArea] public string description = "Add a description here";
    public float cooldown = 0.5f;
    public int manaCost = 5;
    public int weight = 10;

    public BaseAbilityAction[] actions;

    //[Header("Movement")]
    //public bool locksMovement = false;
    //public bool locksFacing = false;
    //public float movementCooldown = 0f;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;

    [field: ReadOnly, NonSerialized] private float currentTime = 0;   
    [field: ReadOnly, NonSerialized] private bool isInCooldown = false;

    [field: ReadOnly, NonSerialized] public int ManaCost { get; set; }
    [field: ReadOnly, NonSerialized] public float Cooldown { get; set; }
    public float CurrentTimeLeft => currentTime;
    public bool IsInCooldown => isInCooldown;
    public int Weight => weight;

    public void Initialize()
    {
        ManaCost = manaCost;
        Cooldown = cooldown;

        currentTime = 0;

        foreach (var action in actions)
            action.Initialize();
    }

    public void Execute(PlayerModel playerModel)
    {
        SetCooldown();

        foreach (var action in actions)
            action.Execute(playerModel);
    }

    private void SetCooldown()
    {
        isInCooldown = true;
        currentTime = Cooldown;
    }

    public void Refresh()
    {
        if (!isInCooldown) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            isInCooldown = false;
    }

    public bool CanBeUsed(int currentMana)
    {
        return !isInCooldown && HasMana(currentMana);
    }

    public bool HasMana(int currentMana)
    {
        return currentMana >= ManaCost;
    }

    public string GetDescription()
    {
        return description;
    }
}
