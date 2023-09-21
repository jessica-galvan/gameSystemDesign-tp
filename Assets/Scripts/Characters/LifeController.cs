using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [field: SerializeField] public bool Invincible { get; private set; }
    [field: SerializeField, ReadOnly] public int CurrentLife { get; private set; }
    [field: SerializeField, ReadOnly] public int MaxLife { get; private set; }
    [field: SerializeField, ReadOnly] public bool Alive { get; private set; }
    /// <summary>
    /// Gives current and max hp
    /// </summary>
    public Action<int, int> OnLifeUpdate;
    public Action OnDeath;

    public void Initialize(CharacterDataSO baseStats)
    {
        Alive = true;
        MaxLife = baseStats.maxLife;
        CurrentLife = MaxLife;
    }

    public void TakeDamage(int damage)
    {
        if (!Alive || Invincible) return;

        CurrentLife -= damage;

        if (CurrentLife <= 0)
            Die();
        else
            OnLifeUpdate?.Invoke(CurrentLife, MaxLife);
    }

    public void Heal(int heal)
    {
        if (CurrentLife == MaxLife) return;
        CurrentLife = Mathf.Clamp(CurrentLife + heal, 0, MaxLife);
        OnLifeUpdate?.Invoke(CurrentLife, MaxLife);
    }

    public bool CanHeal()
    {
        return CurrentLife < MaxLife;
    }

    public void Die()
    {
        if(!Alive || Invincible) return;
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
