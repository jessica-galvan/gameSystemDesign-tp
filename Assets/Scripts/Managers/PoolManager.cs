using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [ReadOnly] public Pool playerBulletPool;
    [ReadOnly] public Pool enemyPool;
    [ReadOnly] public Pool deathParticlePool;
    [ReadOnly] public Pool bulletImpactParticlePool;

    public void Initialize()
    {
        enemyPool = CreateNewPool("EnemyPool", GameManager.Instance.prefabReferences.enemyPrefab.gameObject, GameManager.Instance.globalConfig.maxEnemiesAtAllTimes);
        playerBulletPool = CreateNewPool("PlayerBasicProjectilePool", GameManager.Instance.prefabReferences.playerBasicAttackPrefab.gameObject, GameManager.Instance.globalConfig.initialPool);
        //deathParticlePool = CreateNewPool("DeathParticlePool", GameManager.Instance.prefabReferences.deathParticle.gameObject, GameManager.Instance.globalConfig.particlePool);
        //bulletImpactParticlePool = CreateNewPool("BulletImpactParticlePool", GameManager.Instance.prefabReferences.bulletImpactParticle.gameObject, GameManager.Instance.globalConfig.particlePool);
    }

    private Pool CreateNewPool(string name, GameObject prefab, int initialPoolAmount)
    {
        var container = new GameObject(name);
        Pool newPool = container.AddComponent<Pool>();
        newPool.Initialize(prefab, initialPoolAmount);
        return newPool;
    }


    //TODO rethink this to work with an ID???
    public ProjectileController GetProjectile(ProjectileType type)
    {
        IPoolable bullet = null;

        switch (type)
        {
            case ProjectileType.Basic:
                bullet = playerBulletPool.Spawn();
                break;
            default:
                break;
        }

        return (ProjectileController) bullet;
    }

    public void ReturnBullet(ProjectileController projectile)
    {
        IPoolable poolable = (IPoolable) projectile;

        switch (projectile.data.type)
        {
            case ProjectileType.Basic:
                playerBulletPool.BackToPool(poolable);
                break;
            default:
                break;
        }
    }

    public EnemyController GetEnemy()
    {
        return (EnemyController)enemyPool.Spawn();
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        enemyPool.BackToPool(enemy);
    }

    //public ParticleController GethParticle(ParticleController.ParticleType type)
    //{
    //    ParticleController particle = null;
    //    switch (type)
    //    {
    //        case ParticleController.ParticleType.BulletImpact:
    //            particle = (ParticleController)bulletImpactParticlePool.Spawn();
    //            break;
    //        case ParticleController.ParticleType.Death:
    //            particle = (ParticleController)deathParticlePool.Spawn();
    //            break;
    //    }
    //    return particle;
    //}

    //public void ReturnParticle(ParticleController particle)
    //{
    //    switch (particle.type)
    //    {
    //        case ParticleController.ParticleType.BulletImpact:
    //            bulletImpactParticlePool.BackToPool(particle);
    //            break;
    //        case ParticleController.ParticleType.Death:
    //            deathParticlePool.BackToPool(particle);
    //            break;
    //    }
    //}
}
