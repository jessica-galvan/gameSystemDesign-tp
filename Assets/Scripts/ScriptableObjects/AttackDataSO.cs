using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "TP/Damage/AttackData", order = 3)]
public class AttackDataSO : ScriptableObject
{
    public int damage = 0;
    public bool applyKnockback = true;
    public float force = 0f;

    [NonSerialized] public int Damage;

    public void Initialize()
    {
        Damage = damage;
    }

    public void PowerUp(float attackMultiplier)
    {
        if(attackMultiplier > 0)
            Damage = Mathf.RoundToInt(Damage * attackMultiplier);
    }
}
