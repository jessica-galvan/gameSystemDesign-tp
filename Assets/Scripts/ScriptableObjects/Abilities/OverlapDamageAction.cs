using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "OverlapDamageAction", menuName = "TP/AbilityData/OverlapDamageAction", order = 3)]
public class OverlapDamageAction : BaseAbilityAction
{
    public AttackDataSO baseAttackData;
    public float radius;
    public LayerMask layerMask;

    [Header("VFX")]
    public ParticleSystem particleSystem;
    public float speed = 10f;
    public Color color;
    private MainModule mainModule;

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


        if(particleSystem != null)
        {
            var particle = Instantiate(particleSystem, playerModel.transform);
            particle.transform.localScale = Vector3.one * Radius * 2;
            particle.transform.position = playerModel.transform.position;
            particle.Play();
        }

        Debug.Log($"Ability {name} was executed");
    }

    public override void PowerUp(float amountMultiplier, float attackMultiplier)
    {
        if(amountMultiplier > 0)
            Radius = Mathf.RoundToInt(Radius * amountMultiplier);

        if(attackMultiplier > 0)
            Damage = Mathf.RoundToInt(Damage * attackMultiplier);
    }

    public override void GetDescriptionForPowerUp(StringBuilder stringBuilder, PowerUpAbilitySO powerUp)
    {
        if (powerUp.AmountMultiplier > 0)
            stringBuilder.AppendLine($"- Gains an {powerUp.AmountMultiplier * 10}% of extra area of damage");

        if (powerUp.AttackMultiplier > 0)
            stringBuilder.AppendLine($"- Gains an {powerUp.AttackMultiplier * 10}% of extra damage");
    }
}