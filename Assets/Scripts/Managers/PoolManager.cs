using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [ReadOnly] public Pool playerBulletPool;
    [ReadOnly] public Pool enemyPool;
    [ReadOnly] public Pool deathParticlePool;
    [ReadOnly] public Pool manaDropPool;

    //[ReadOnly] public Pool bulletImpactParticlePool;

    public void Initialize()
    {
        enemyPool = CreateNewPool("EnemyPool", GameManager.Instance.prefabReferences.enemyPrefab.gameObject, GameManager.Instance.globalConfig.maxEnemiesAtAllTimes);
        playerBulletPool = CreateNewPool("PlayerBasicProjectilePool", GameManager.Instance.prefabReferences.playerBasicAttackPrefab.gameObject, GameManager.Instance.globalConfig.initialPool);
        manaDropPool = CreateNewPool("ManaDroplePool", GameManager.Instance.prefabReferences.manaDropPrefab.gameObject, GameManager.Instance.globalConfig.initialPool);
        deathParticlePool = CreateNewPool("DeathParticlePool", GameManager.Instance.prefabReferences.deathVFX.gameObject, GameManager.Instance.globalConfig.initialPool);

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


    public ManaDrop GetManaDrop()
    {
        return (ManaDrop)manaDropPool.Spawn();
    }

    public void ReturnManaDrop(ManaDrop drop)
    {
        manaDropPool.BackToPool(drop);
    }

    public ParticleEffect GetParticle(ParticleEffect.ParticleType Type)
    {
        ParticleEffect particle = null;
        switch (Type)
        {
            case ParticleEffect.ParticleType.OnHit:
                //particle = (ParticleEffect)bulletImpactParticlePool.Spawn();
                break;
            case ParticleEffect.ParticleType.Death:
                particle = (ParticleEffect)deathParticlePool.Spawn();
                break;
        }
        return particle;
    }

    public void ReturnParticle(ParticleEffect particle)
    {
        switch (particle.Type)
        {
            case ParticleEffect.ParticleType.OnHit:
                //bulletImpactParticlePool.BackToPool(particle);
                break;
            case ParticleEffect.ParticleType.Death:
                deathParticlePool.BackToPool(particle);
                break;
        }
    }
}
