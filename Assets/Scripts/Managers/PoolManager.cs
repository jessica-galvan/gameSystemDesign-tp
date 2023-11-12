using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PoolManager : MonoBehaviour
{
    [ReadOnly] public Pool playerBulletPool;
    [ReadOnly] public Pool deathParticlePool;
    [ReadOnly] public Pool manaDestructionPool;
    [ReadOnly] public Pool manaDropPool;
    [ReadOnly] public List<Pool> enemyPools = new List<Pool>();

    //[ReadOnly] public Pool bulletImpactParticlePool;

    public void Initialize()
    {
        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            var enemyData = GameManager.Instance.globalConfig.enemySpawnDataList[i];
            if (!enemyData.CanBeSpawned) continue;

            var initialPoolAmount = enemyData.HasMaxLimitRun ? enemyData.maxLimitPerRun : enemyData.HasLimitAmount ? enemyData.limitAmount : GameManager.Instance.globalConfig.initialPool;
            var name = !string.IsNullOrEmpty(enemyData.poolName) ? enemyData.poolName : enemyData.enemyPrefab.gameObject.name;
            var enemyPool = CreateNewPool($"Pool_{name}", enemyData.enemyPrefab.gameObject, initialPoolAmount, name);
            enemyPools.Add(enemyPool);

        }

        playerBulletPool = CreateNewPool("PlayerBasicProjectilePool", GameManager.Instance.prefabReferences.playerBasicAttackPrefab.gameObject, GameManager.Instance.globalConfig.initialPool, "PlayerBullet");
        manaDropPool = CreateNewPool("ManaDroplePool", GameManager.Instance.prefabReferences.manaDropPrefab.gameObject, GameManager.Instance.globalConfig.initialPool, "ManaDrop");
        deathParticlePool = CreateNewPool("DeathParticlePool", GameManager.Instance.prefabReferences.deathVFX.gameObject, GameManager.Instance.globalConfig.initialPool, "DeathVFX");
        manaDestructionPool = CreateNewPool("ManaParticlePool", GameManager.Instance.prefabReferences.manaDestructionVFX.gameObject, GameManager.Instance.globalConfig.initialPool, "ManaDestructionVFX");

        //bulletImpactParticlePool = CreateNewPool("BulletImpactParticlePool", GameManager.Instance.prefabReferences.bulletImpactParticle.gameObject, GameManager.Instance.globalConfig.particlePool);
    }

    private Pool CreateNewPool(string name, GameObject prefab, int initialPoolAmount, string itemName)
    {
        var container = new GameObject(name);
        Pool newPool = container.AddComponent<Pool>();
        newPool.Initialize(prefab, initialPoolAmount, itemName);
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

    public EnemyController GetEnemy(int index)
    {
        if(index < enemyPools.Count)
            return (EnemyController)enemyPools[index].Spawn();
        else
        {
            Debug.LogError($"Index is too high");
            return (EnemyController)enemyPools[0].Spawn();
        }
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        var index = GameManager.Instance.enemyManager.EnemyTypeReturnes(enemy);

        if (enemyPools.Count < index ) 
            enemyPools[index].BackToPool(enemy);
        else
        {
            enemy.gameObject.SetActive(false);
            Debug.LogError($"{enemy.gameObject.name} doesn't seem to have a pool");
        }
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
                particle = (ParticleEffect) deathParticlePool.Spawn();
                break;
            case ParticleEffect.ParticleType.ManaDestruction:
                particle = (ParticleEffect) manaDestructionPool.Spawn();
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
            case ParticleEffect.ParticleType.ManaDestruction:
                manaDestructionPool.BackToPool(particle);
                break;
        }
    }
}
