using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody2D))]
public class BaseCharacterModel : MonoBehaviour, IDamagable
{
    [SerializeField] protected CharacterBaseStatsSO baseStats;
    [ReadOnly, SerializeField] protected Rigidbody2D rb;
    [ReadOnly, SerializeField] protected Vector2 currentDirection;
    [ReadOnly, SerializeField] private bool flipX = false;

    public LifeController LifeController { get; private set; }
    public bool Alive => LifeController.Alive;
    public Vector2 Direction => currentDirection;
    public bool FlipX => flipX;
    public CharacterBaseStatsSO BaseStats => baseStats;

    public virtual void Initialize()
    {
        LifeController = GetComponent<LifeController>();
        rb = GetComponent<Rigidbody2D>();
        LifeController.Initialize(baseStats);
    }

    public void Idle()
    {
        //TODO change animation to idle
    }

    public virtual void Move(Vector2 direction)
    {
        currentDirection = direction;
        rb.AddForce(currentDirection * baseStats.movementSpeed * Time.deltaTime);
        //rb.AddForce(currentDirection * baseStats.movementSpeed * Time.deltaTime);
        //rb.velocity = direction * baseStats.movementSpeed;
    }

    public void LookDirection(Vector2 dir)
    {
        //TODO change sprite direction
        //flipX = dir.x > 0.1f;
    }

    public virtual void TakeDamage(int damage)
    {
        if (LifeController.Invincible) return;
        LifeController.TakeDamage(damage);
    }

    public virtual void TakeDamage(int damage, Vector2 direction)
    {
        if (LifeController.Invincible) return;
        TakeDamage(damage);

        if (Alive && baseStats.canBeKockedBack)
            KnockedBack(direction);
    }

    public void KnockedBack(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}
