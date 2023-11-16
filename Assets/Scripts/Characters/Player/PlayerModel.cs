using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseCharacterModel
{
    [ReadOnly] public ProjectileType basicAttack;
    [SerializeField] private AbilityDataSO[] unlockedAbilities;
    private int currentUnlockedAbilities = 0;

    public Gradient gradientTest;
    public float widthTest = 5f, speedTest = 5f, radiusTest = 5f;

    [SerializeField, ReadOnly] private float cooldown = 0;
    [SerializeField, ReadOnly] private float cooldownShootTimer = 0f;

    [Header("Info")]
    [SerializeField, ReadOnly] private bool isMoving;
    [field: SerializeField, ReadOnly] public bool CanShoot { get; private set; }

    public int UnlockedAbilitiesCounter => unlockedAbilities.Length;

    public Action<AbilityDataSO> OnUnlockedAbilityEvent;

    public override void Initialize(CharacterDataSO stats)
    {
        base.Initialize(stats);

        cooldown = GameManager.Instance.prefabReferences.playerBasicAttackPrefab.data.cooldown;
        unlockedAbilities = new AbilityDataSO[GameManager.Instance.playerData.maxAbilities];
        CanShoot = true;
    }

    public void Shoot(Vector2 mousePos, ProjectileDataSO projectileOverride = null)
    {
        if (!CanShoot)
        {
            //GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.negativeShootSound);
            return;
        }

        var direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        ProjectileController bullet = null;

        if (projectileOverride != null)
            bullet = GameManager.Instance.poolManager.GetProjectile(projectileOverride.type);
        else
            bullet = GameManager.Instance.poolManager.GetProjectile(basicAttack);

        var spawnPoint = rb.position + (direction.normalized * baseStats.radius);
        bullet.SetDirection(spawnPoint, direction, angle);
        CanShoot = false;
        cooldownShootTimer = 0f;

       GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.playerShootSound);
    }

    public override void Move(Vector2 direction)
    {
        currentDirection = direction.normalized;
        rb.velocity = currentDirection * baseStats.MovementSpeed * Time.deltaTime;

        UpdateMovementAnimation();
        Flip(currentDirection);
    }

    public void UpdateMovementAnimation()
    {
        var moving = Mathf.Abs(rb.velocity.x) > float.Epsilon || Mathf.Abs(rb.velocity.y) > float.Epsilon;

        if (isMoving == moving) return;
        isMoving = moving;

        animator.SetBool("isMoving", isMoving);
    }

    public void ShootingCooldown()
    {
        if (CanShoot) return;

        cooldownShootTimer += Time.deltaTime;

        //TODO rethink this for the ability cooldown
        if (cooldownShootTimer >= cooldown)
            CanShoot = true;
    }

    public void RefreshAbilities()
    {
        for (int i = 0; i < currentUnlockedAbilities; i++)
            unlockedAbilities[i].Refresh();
    }

    public void UnlockAbility(AbilityDataSO abilityData)
    {
        if(!CanUnlockAbility())
        {
            Debug.LogError($"Trying to unlock more abilities than needed. CurrentAbilities {currentUnlockedAbilities}");
            return;
        }

        unlockedAbilities[currentUnlockedAbilities] = abilityData;
        currentUnlockedAbilities++;

        OnUnlockedAbilityEvent?.Invoke(abilityData);
    }

    public bool CanUnlockAbility()
    {
        return currentUnlockedAbilities < GameManager.Instance.playerData.maxAbilities;
    }


    public bool CanUseAbility(int index)
    {
        if (unlockedAbilities.Length <= index) return false;
        if (unlockedAbilities[index] == null) return false;

        return unlockedAbilities[index].CanBeUsed(GameManager.Instance.manaSystem.currentMana);
    }

    public void UseAbility(int index)
    {
        GameManager.Instance.manaSystem.RemoveMana(unlockedAbilities[index].ManaCost);
        unlockedAbilities[index].Execute(this);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!CanTakeDamage) return;

        if(collision.gameObject.TryGetComponent<IDamage>(out var idamage))
            TakeDamage((int)idamage.AttackData.Damage);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
            collectable.PickUp();
    }

    public void SetNewSpeed(float newSpeed)
    {
        baseStats.ChangeSpeed(newSpeed);
    }

    public void SetNewBaseAttackCooldown(float newCooldown)
    {
        cooldown = newCooldown;
    }

    public override void TakeDamage(int damage)
    {
        if (!CanTakeDamage) return;
        base.TakeDamage(damage);
        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.playerTakeDamageSound);
    }
}
