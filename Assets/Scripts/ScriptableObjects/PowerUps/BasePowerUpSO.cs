using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "TP/PowerUp/PowerUpBase", order = 0)]
public abstract class BasePowerUpSO : ScriptableObject, ISelectableOption, IWeight
{
    [Header("Basic info")]
    [SerializeField] protected string title = "TITLE";
    [SerializeField] protected Sprite icon;
    [TextArea, SerializeField] protected string description = "Add a description here";
    [SerializeField] protected int weight = 10;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;
    public int Weight => weight;

    protected string newDescription;

    public abstract void Initialize();

    public abstract void Execute();

    public abstract string GetDescription();
}
