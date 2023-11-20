using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseCharacterModel
{
    [ReadOnly] public ProjectileType basicAttack;
    [ReadOnly] private ProjectileDataSO projectileData;
    [SerializeField] private AbilityDataSO[] unlockedAbilities;
    private int currentUnlockedAbilities = 0;

    [SerializeField, ReadOnly] private float cooldownShootTimer = 0f;

    [Header("Info")]
    [SerializeField, ReadOnly] private bool isMoving;
    [field: SerializeField, ReadOnly] public bool CanShoot { get; private set; }

    public ProjectileDataSO ProjectileData => projectileData;
    public int UnlockedAbilitiesCounter => unlockedAbilities.Length;

    public Action<AbilityDataSO> OnUnlockedAbilityEvent;

    public override void Initialize(CharacterDataSO stats)
    {
        stats.Initialize();
        base.Initialize(stats);

        Debug.Assert(LifeController.CurrentLife == LifeController.MaxLife, $"Player life({LifeController.CurrentLife}) is less than max life ({LifeController.MaxLife})");
        Debug.Log($"Player Start Life({LifeController.CurrentLife})");

        projectileData = GameManager.Instance.prefabReferences.playerBasicAttackPrefab.data;
        projectileData.Initialize();
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
        if (cooldownShootTimer >= projectileData.Cooldown)
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
        if (!Alive) return;
        if (!CanTakeDamage) return;

        if(collision.gameObject.TryGetComponent<IDamage>(out var idamage))
            TakeDamage(idamage.AttackData.Damage, ignoreCooldown: false);
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

    public override void TakeDamage(int damage, bool ignoreCooldown = true)
    {
        if (!Alive) return;
        if (!CanTakeDamage) return;
        Debug.Log($"Player takes damage {damage}. CurrentLife: {LifeController.CurrentLife}");
        base.TakeDamage(damage, ignoreCooldown);
        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.playerTakeDamageSound);
    }

    public override void TakeDamage(int damage, Vector2 direction, bool ignoreCooldown = true)
    {
        if (!Alive) return;
        if (!CanTakeDamage) return;
        Debug.Log($"Player takes damage {damage}. CurrentLife: {LifeController.CurrentLife}");
        base.TakeDamage(damage, direction, ignoreCooldown);
        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.playerTakeDamageSound);
    }
}
