using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "PowerUpData", menuName = "TP/PowerUp/PowerUpBase", order = 0)]
public abstract class BasePowerUpSO : ScriptableObject, ISelectableOption, IWeight
{
    [Header("Basic info")]
    [SerializeField] protected string title = "TITLE";
    [SerializeField] protected Sprite icon;
    [SerializeField] protected int weight = 10;
    [SerializeField] protected int reutilizableTimes = 0;

    public string Title => title;
    public Sprite Icon => icon;
    public int Weight => weight;

    [NonSerialized] protected int currentTimes = 0;

    public abstract void Initialize();

    public virtual void Execute()
    {
        AddCurrentUse();
    }

    public abstract string GetDescription();

    protected void AddCurrentUse()
    {
        currentTimes++;
    }

    public bool CanBeReutilized()
    {
        return currentTimes < reutilizableTimes;
    }

    public bool IsPowerUp()
    {
        return true;
    }

    public void SetOverrideIcon(Sprite sprite)
    {
        icon = sprite;
    }
}
