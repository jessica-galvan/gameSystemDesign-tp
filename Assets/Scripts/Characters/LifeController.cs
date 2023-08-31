using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField, ReadOnly] private CharacterBaseStatsSO baseStats;

    [field: SerializeField, ReadOnly] public int CurrentLife { get; private set; }
    [field: SerializeField, ReadOnly] public int MaxLife { get; private set; }
    [field: SerializeField, ReadOnly] public bool Alive { get; private set; }

    public Action OnLifeUpdate;
    public Action OnDeath;

    public void Initialize(CharacterBaseStatsSO baseStats)
    {
        this.baseStats = baseStats;

        Alive = true;
        MaxLife = baseStats.maxLife;
        CurrentLife = MaxLife;
    }

    public void TakeDamage(int damage)
    {
        if (!Alive) return;

        CurrentLife -= damage;

        if (CurrentLife <= 0)
            Die();
        else
            OnLifeUpdate?.Invoke();
    }

    public void Heal(int heal)
    {
        if (CurrentLife == MaxLife) return;
        CurrentLife = Mathf.Clamp(CurrentLife + heal, 0, MaxLife);
        OnLifeUpdate?.Invoke();
    }

    public bool CanHeal()
    {
        return CurrentLife < MaxLife;
    }

    public void Die()
    {
        if(!Alive) return;
        Alive = false;
        CurrentLife = 0;
        OnDeath?.Invoke();
    }

    public void ResetStats()
    {
        Alive = true;
        CurrentLife = MaxLife;
    }
}
