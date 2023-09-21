using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseCharacterModel
{
    public ProjectileType basicAttack;

    [SerializeField] private AbilityDataSO[] unlockedAbilities;

    private int unlockedAbilitiesCounter;
    private float cooldownShootTimer = 0f;

    [field: SerializeField, ReadOnly] public bool CanShoot { get; private set; }

    public Action<AbilityDataSO> OnUnlockedAbilityEvent;

    public override void Initialize()
    {
        base.Initialize();

        unlockedAbilities = new AbilityDataSO[GameManager.Instance.playerData.maxAbilities];
        CanShoot = true;
    }

    public void Shoot(Vector2 mousePos)
    {
        if (!CanShoot)
        {
            GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.negativeShootSound);
            return;
        }

        var direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        var bullet = GameManager.Instance.poolManager.GetProjectile(basicAttack);
        var spawnPoint = rb.position + (direction.normalized * baseStats.radius);
        bullet.SetDirection(spawnPoint, direction, angle);
        CanShoot = false;
        cooldownShootTimer = 0f;

        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.playerShoot);
    }

    public override void Move(Vector2 direction)
    {
        currentDirection = direction;
        rb.velocity = currentDirection * baseStats.movementSpeed * Time.deltaTime;
        //rb.AddForce(currentDirection * baseStats.movementSpeed * Time.deltaTime);
    }

    public void ShootingCooldown()
    {
        if (CanShoot) return;

        cooldownShootTimer += Time.deltaTime;

        //TODO rethink this for the ability cooldown
        if (cooldownShootTimer >= GameManager.Instance.prefabReferences.playerBasicAttackPrefab.data.cooldown)
            CanShoot = true;
    }

    public void UnlockAbility(AbilityDataSO abilityData)
    {
        Debug.Assert(unlockedAbilitiesCounter < GameManager.Instance.playerData.maxAbilities, "Trying to unlock more abilities than needed");
        unlockedAbilities[unlockedAbilitiesCounter] = abilityData;
        unlockedAbilitiesCounter++;
        OnUnlockedAbilityEvent?.Invoke(abilityData);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO rethink this with cooldown and make if you stay in contact?
        if(collision.gameObject.TryGetComponent<EnemyModel>(out var enemy))
            TakeDamage((int)enemy.attackStats.damage);
            //TakeDamage((int)enemy.attackStats.damage, enemy.Direction * enemy.attackStats.force);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
            collectable.PickUp();
    }
}
