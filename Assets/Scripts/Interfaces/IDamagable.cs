using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public LifeController LifeController { get; }
    public bool Alive { get; }

    void TakeDamage(int damage, Vector2 direction, float force, ForceMode2D forceMode);
}
