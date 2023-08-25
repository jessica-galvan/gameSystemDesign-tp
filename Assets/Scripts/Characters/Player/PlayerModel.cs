using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseCharacterModel
{
    //Shooting
    protected float cooldownShootTimer = 0f;
    protected float currentStuckCounter = 0f;
    [field: SerializeField, ReadOnly] public bool CanShoot { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        CanShoot = true;
    }

    public void Shoot(Vector2 mousePos)
    {
        if (!CanShoot)
        {
            AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.negativeShootSound);
            return;
        }

        var direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        //var bullet = GameManager.Instance.poolManager.GetBullet(entityConfig.bulletType);
        var bullet = Instantiate(GameManager.Instance.prefabReferences.testingBulletPrefab);
        bullet.Initialize();

        var spawnPoint = rb.position + (direction.normalized * baseStats.radius);
        bullet.SetDirection(spawnPoint, direction, angle);
        CanShoot = false;
        cooldownShootTimer = 0f;

        AudioManager.instance.PlaySFXSound(AudioManager.instance.soundReferences.playerShoot);
    }


    public void ShootingCooldown()
    {
        if (CanShoot) return;

        cooldownShootTimer += Time.deltaTime;

        //TODO rethink this for the ability cooldown
        if (cooldownShootTimer >= GameManager.Instance.prefabReferences.testingBulletPrefab.data.cooldown)
            CanShoot = true;
    }
}
