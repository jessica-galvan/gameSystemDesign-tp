using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlapDamageAction", menuName = "TP/AbilityData/OverlapDamageAction", order = 3)]
public class OverlapDamageAction : BaseAbilityAction
{
    public AttackDataSO baseAttackData;
    public float radius;
    public LayerMask layerMask;

    [field: ReadOnly, NonSerialized] public int Damage { get; set; }
    [field: ReadOnly, NonSerialized] public float Force { get; set; }
    [field: ReadOnly, NonSerialized] public float Radius { get; set; }
    [field: ReadOnly, NonSerialized] public bool ApplyKnockback { get; set; }

    public override void Initialize()
    {
        Damage = baseAttackData.damage;
        Force = baseAttackData.force;
        ApplyKnockback = baseAttackData.applyKnockback;
        Radius = radius;
    }

    public override void Execute(PlayerModel playerModel)
    {
        var hits = Physics2D.OverlapCircleAll(playerModel.transform.position, Radius, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            var damagable = hits[i].GetComponent<IDamagable>();
            if (damagable == null) continue;

            if (ApplyKnockback)
            {
                var direction = (hits[i].transform.position - playerModel.transform.position).normalized;
                damagable.TakeDamage(Damage, direction * Force);
            }
            else
                damagable.TakeDamage(Damage);
        }

        Debug.Log($"Ability {name} was executed");
    }
}