using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseCharacterModel
{
    //Shooting
    protected bool canShoot = true;
    protected float cooldownShootTimer = 0f;
    protected float currentStuckCounter = 0f;

    public override void Initialize()
    {
        base.Initialize();

    }

    public void ShootingCooldown()
    {
        if (canShoot) return;

        cooldownShootTimer += Time.deltaTime;

        if (cooldownShootTimer >= baseStats.shootingCooldown)
            canShoot = true;
    }
}
