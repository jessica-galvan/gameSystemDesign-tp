using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody2D))]
public class BaseCharacterModel : MonoBehaviour
{
    [SerializeField] protected CharacterBaseStatsSO baseStats;
    [ReadOnly, SerializeField] protected Rigidbody2D rb;
    [ReadOnly, SerializeField] protected Vector2 direction;

    public LifeController LifeController { get; private set; }
    public bool Alive => LifeController.Alive;
    public Vector2 Direction => direction;

    public virtual void Initialize()
    {
        LifeController = GetComponent<LifeController>();
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
    }

    public void LookDirection(Vector2 dir)
    {
        //TODO change sprite direction
    }
}
