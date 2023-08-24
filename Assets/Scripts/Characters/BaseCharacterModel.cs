using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody2D))]
public class BaseCharacterModel : MonoBehaviour
{
    [SerializeField] protected CharacterBaseStatsSO baseStats;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Vector2 direction;

    public LifeController LifeController { get; private set; }
    public bool Alive => LifeController.Alive;

    public virtual void Initialize()
    {
        LifeController = GetComponent<LifeController>();
        LifeController.Initialize(baseStats);
    }

    public void Idle()
    {
        rb.velocity = Vector3.zero;
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * baseStats.movementSpeed;
    }

    public void LookDirection(Vector2 dir)
    {
        //TODO change sprite direction
    }
}
