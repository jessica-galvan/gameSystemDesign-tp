using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public int currentMana;
    public int maxMana;

    public float CurrentT { get; private set; }

    /// <summary>
    /// Gives fillAmount, currentMana, maxMana
    /// </summary>
    public Action<float, int, int> OnUpdateMana;

    public void Initialize()
    {
        maxMana = GameManager.Instance.playerData.maxMana;
        currentMana = 0;
        RecalculateCurrentT();
    }


    public void AddMana(int mana)
    {
        currentMana += mana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        RecalculateCurrentT();
    }

    public bool CanCast(int manaCost)
    {
        return manaCost <= currentMana;
    }

    public void RemoveMana(int mana)
    {
        Debug.Assert(currentMana >= mana, "Trying to remove more mana than what player has");
        currentMana -= mana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        RecalculateCurrentT();
    }

    public void SetMaxMana(int newMaxMana)
    {
        maxMana = newMaxMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        RecalculateCurrentT();
    }

    private void RecalculateCurrentT()
    {
        CurrentT = Mathf.InverseLerp(0, maxMana, currentMana);
        OnUpdateMana?.Invoke(CurrentT, currentMana, maxMana);
    }
}
