using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody2D))]
public class BaseCharacterModel : MonoBehaviour, IDamagable
{
    [SerializeField] protected CharacterBaseStatsSO baseStats;
    [ReadOnly, SerializeField] protected Rigidbody2D rb;
    [ReadOnly, SerializeField] protected Vector2 direction;
    [ReadOnly, SerializeField] private bool flipX = false;

    public bool wasKnocked = false;

    public LifeController LifeController { get; private set; }
    public bool Alive => LifeController.Alive;
    public Vector2 Direction => direction;
    public bool FlipX => flipX;
    public CharacterBaseStatsSO BaseStats => baseStats;

    public Action OnBeingKocked;

    public virtual void Initialize()
    {
        LifeController = GetComponent<LifeController>();
        rb = GetComponent<Rigidbody2D>();
        LifeController.Initialize(baseStats);
    }

    public void Idle()
    {
        rb.velocity = Vector2.zero;
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
        rb.velocity = direction * baseStats.movementSpeed;
        //var pos = (Vector2)transform.position + direction * baseStats.movementSpeed;
        //rb.MovePosition(pos);
    }

    public void LookDirection(Vector2 dir)
    {
        //TODO change sprite direction
        //flipX = dir.x > 0.1f;
    }

    public virtual void TakeDamage(int damage, Vector2 direction, float force, ForceMode2D forceMode)
    {
        LifeController.TakeDamage(damage);

        if (Alive && baseStats.canBeKockedBack)
            KnockedBack(direction, force, forceMode);
    }

    public void KnockedBack(Vector2 direction, float force, ForceMode2D forceMode)
    {
        wasKnocked = true;
        rb.AddForce(direction * force, forceMode);
        OnBeingKocked?.Invoke();
    }
}
