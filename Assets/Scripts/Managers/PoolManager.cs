using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PoolManager : MonoBehaviour
{
    [ReadOnly] public Pool<ProjectileController> playerBulletPool;
    [ReadOnly] public Pool<ParticleEffect> deathParticlePool;
    [ReadOnly] public Pool<ParticleEffect> manaDestructionPool;
    [ReadOnly] public Pool<ManaDrop> manaDropPool;
    [ReadOnly] public Pool<DamageNumbers> damageNumbersPool;
    [ReadOnly] public Dictionary<int, Pool<EnemyController>> enemyPools = new Dictionary<int, Pool<EnemyController>>();

    //[ReadOnly] public Pool bulletImpactParticlePool;

    public void Initialize()
    {
        for (int i = 0; i < GameManager.Instance.globalConfig.allEnemies.Length; i++)
        {
            var enemy = GameManager.Instance.globalConfig.allEnemies[i];
            var enemyPool = CreateNewPool($"Pool_{name}", enemy, GameManager.Instance.globalConfig.enemyInitialPool, enemy.gameObject.name, OnEnemyCreated, initStartingSize: false);
            enemyPools.Add(enemy.ID, enemyPool);
        }

        foreach (var item in enemyPools)
            item.Value.InstantiateSartingSize();


        playerBulletPool = CreateNewPool("PlayerBasicProjectilePool", GameManager.Instance.prefabReferences.playerBasicAttackPrefab, GameManager.Instance.globalConfig.initialPool, "PlayerBullet");
        manaDropPool = CreateNewPool("ManaDroplePool", GameManager.Instance.prefabReferences.manaDropPrefab, GameManager.Instance.globalConfig.initialPool, "ManaDrop");
        deathParticlePool = CreateNewPool("DeathParticlePool", GameManager.Instance.prefabReferences.deathVFX, GameManager.Instance.globalConfig.initialPool, "DeathVFX");
        manaDestructionPool = CreateNewPool("ManaParticlePool", GameManager.Instance.prefabReferences.manaDestructionVFX, GameManager.Instance.globalConfig.initialPool, "ManaDestructionVFX");
        damageNumbersPool = CreateNewPool("DamageVFXPool", GameManager.Instance.prefabReferences.damageTextVFX, GameManager.Instance.globalConfig.initialPool, "DamageVFX");

        //bulletImpactParticlePool = CreateNewPool("BulletImpactParticlePool", GameManager.Instance.prefabReferences.bulletImpactParticle.gameObject, GameManager.Instance.globalConfig.particlePool);
    }

    private Pool<T> CreateNewPool<T>(string name, T prefab, int initialPoolAmount, string itemName, Action<T, Pool<T>> OnCreate = null, bool initStartingSize = true)
        where T:  MonoBehaviour, IPoolable
    {
        var container = new GameObject(name);
        Pool<T> newPool = new Pool<T>();
        newPool.Initialize(prefab, initialPoolAmount, itemName, container.transform, OnCreate, initStartingSize);
        return newPool;
    }

    private void OnEnemyCreated(EnemyController enemyController, Pool<EnemyController> pool)
    {

    }

    //TODO rethink this to work with an ID???
    public ProjectileController GetProjectile(ProjectileType type)
    {
        ProjectileController bullet = null;

        switch (type)
        {
            case ProjectileType.Basic:
                bullet = playerBulletPool.Spawn();
                break;
            default:
                break;
        }

        return bullet;
    }

    public void ReturnBullet(ProjectileController projectile)
    {
        switch (projectile.data.type)
        {
            case ProjectileType.Basic:
                playerBulletPool.BackToPool(projectile);
                break;
            default:
                break;
        }
    }

    public EnemyController GetEnemy(int index)
    {
        if(index < enemyPools.Count)
            return enemyPools[index].Spawn();
        else
        {
            Debug.LogError($"Index is too high");
            return enemyPools[0].Spawn();
        }
    }

    public void ReturnEnemy(EnemyController enemy)
    {
        if(enemyPools.TryGetValue(enemy.ID, out var enemyPool))
            enemyPool.BackToPool(enemy);
        else
        {
            Debug.LogError($"{enemy.gameObject.name} doesn't seem to havxe a pool");
            enemy.gameObject.SetActive(false);
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
                particle = deathParticlePool.Spawn();
                break;
            case ParticleEffect.ParticleType.ManaDestruction:
                particle = manaDestructionPool.Spawn();
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

    public DamageNumbers GetDamageEffect()
    {
        return damageNumbersPool.Spawn();
    }

    public void ReturnDamageEffect(DamageNumbers effect)
    {
        damageNumbersPool.BackToPool(effect);
    }
}
