using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public LifeController LifeController { get; }
    public bool Alive { get; }

    void TakeDamage(int damage, bool ignoreCooldown = true);
    void TakeDamage(int damage, Vector2 direction, bool ignoreCooldown = true);
}

public interface IDamage
{
    AttackDataSO AttackData { get; }
}

public interface IDirection
{
    void SetDirection(Vector2 startingPosition, Vector2 direction, float rotation = 0f);
}
