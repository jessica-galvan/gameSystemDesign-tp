using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "TP/AbilityData/AbilityData", order = 3)]
public class AbilityDataSO : ScriptableObject, ISelectableOption
{
    [Header("Basic info")]
    public string title = "TITLE";
    public Sprite icon;
    [TextArea] public string description = "Add a description here";
    public float cooldown = 0.5f;
    public int manaCost = 5;

    public BaseAbilityAction[] actions;

    //[Header("Movement")]
    //public bool locksMovement = false;
    //public bool locksFacing = false;
    //public float movementCooldown = 0f;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;

    [field: ReadOnly, NonSerialized] public int ManaCost { get; set; }
    [field: ReadOnly, NonSerialized] public float Cooldown { get; set; }

    public void Initialize()
    {
        ManaCost = manaCost;
        Cooldown = cooldown;

        foreach (var action in actions)
            action.Initialize();
    }

    public void Execute(BaseCharacterModel characterModel)
    {
        foreach (var action in actions)
            action.Execute(characterModel);
    }
}
