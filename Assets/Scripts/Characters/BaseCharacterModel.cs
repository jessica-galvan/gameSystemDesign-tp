using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController), typeof(Rigidbody2D))]
public class BaseCharacterModel : MonoBehaviour, IDamagable
{
    [SerializeField] protected CharacterDataSO baseStats;
    [SerializeField] protected ParticleSystem[] takeDamageVFX;
    [ReadOnly, SerializeField] protected SpriteRenderer spriteRenderer;
    [ReadOnly, SerializeField] protected Rigidbody2D rb;
    [ReadOnly, SerializeField] protected Animator animator;
    [ReadOnly, SerializeField] protected Vector2 currentDirection;
    [ReadOnly, SerializeField] private bool flipX = false;

    protected Color originalColor;

    protected bool canTakeDamage = true;
    protected bool isRecoloredDamage = false;
    protected float currentRecolorTimer = 0f;
    protected float currentTakeDamageCooldown = 0f;

    public LifeController LifeController { get; private set; }
    public bool Alive => LifeController.Alive;
    public Vector2 Direction => currentDirection;
    public bool FlipX => flipX;
    public CharacterDataSO BaseStats => baseStats;
    public Animator Animator => animator;

    public virtual void Initialize()
    {
        LifeController = GetComponent<LifeController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        LifeController.Initialize(baseStats);

        originalColor = spriteRenderer.color;
    }

    public void Refresh(float deltaTime)
    {
        if (!canTakeDamage)
        {
            currentTakeDamageCooldown -= deltaTime;
            if (currentTakeDamageCooldown <= 0)
                canTakeDamage = true;
        }

        if (isRecoloredDamage)
        {
            currentRecolorTimer -= deltaTime;
            if (currentRecolorTimer <= 0)
                SetVisualTakeDamage(false);
        }
    }

    public void Idle()
    {
        //TODO change animation to idle
    }

    public virtual void Move(Vector2 direction)
    {
        currentDirection = direction;
        rb.AddForce(currentDirection * baseStats.movementSpeed * Time.deltaTime);

        Flip(currentDirection);

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
        if (!canTakeDamage) return;

        LifeController.TakeDamage(damage);

        for (int i = 0; i < takeDamageVFX.Length; i++)
            takeDamageVFX[i].Play();

        if (baseStats.takeDamageCooldown > 0)
        {
            canTakeDamage = false;
            currentTakeDamageCooldown = baseStats.takeDamageCooldown;
        }

        SetVisualTakeDamage(true);
    }

    protected void SetVisualTakeDamage(bool isDamaged)
    {
        if (isDamaged)
        {
            isRecoloredDamage = true;
            currentRecolorTimer = baseStats.takeDamageRecolorTime;
            spriteRenderer.color = baseStats.takeDamageColor;
        }
        else
        {
            if (isDamaged == isRecoloredDamage) return;
            isRecoloredDamage = false;
            spriteRenderer.color = originalColor;
        }
    }

    public virtual void TakeDamage(int damage, Vector2 direction)
    {
        if (LifeController.Invincible) return;
        if (!canTakeDamage) return;

        TakeDamage(damage);

        if (Alive && baseStats.canBeKockedBack)
            KnockedBack(direction);
    }

    public void KnockedBack(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x == 0) return;

        var flipped = direction.x < 0;

        if (flipX == flipped) return;
        flipX = flipped;

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipX ? 180f : 0f, 0f));
    }
}
