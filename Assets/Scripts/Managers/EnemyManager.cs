using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct EnemySpawnData
{
    public EnemyController enemyPrefab;
    public int weight;
    [Tooltip("How many can be in the scene at the same time. If 0, there is no limit")]
    public int limitAmount;
    [Tooltip("How many can appear in the same run. If 0, there is no limit")]
    public int maxLimitPerRun;

    public bool HasMaxLimitRun => maxLimitPerRun > 0;
    public bool HasLimitAmount => limitAmount > 0;
}

public class EnemyManager : MonoBehaviour, IUpdate
{
    [ReadOnly] public int totalKilled = 0;
    [ReadOnly] public int totalSpawned = 0;
    [ReadOnly] public int currentEnemyQuantitySpawned = 0;
    [ReadOnly, SerializeField] private bool canSpawnEnemies;

    public Action<int> OnEnemyKilled = delegate { };
    private float currentTime;
    private List<EnemyController> inLevelEnemies = new List<EnemyController>();

    public void Initialize()
    {
        canSpawnEnemies = true;
        GameManager.Instance.updateManager.uncappedCustomUpdate.Add(this);
    }

    public void Refresh(float deltaTime)
    {
        if (!GameManager.Instance.CanUpdate) return;

        if (canSpawnEnemies)
        {
            currentTime -= deltaTime;

            if (currentTime <= 0)
                EnemySpawn();
        }

        //This is to limit if enemies go to far, we realocated them near 
        if (GameManager.Instance.globalConfig.respawnEnemiesWhenTooFar)
        {
            for (int i = 0; i < inLevelEnemies.Count; i++)
            {
                if (IsInsideDesignatedArea(inLevelEnemies[i].transform.position)) continue;
                inLevelEnemies[i].transform.position = GetSpawnPos(extraRandomness: true);
            }
        }
    }


    private bool IsInsideDesignatedArea(Vector3 currentEnemyPos)
    {
        Vector3 distance = GameManager.Instance.Player.transform.position - currentEnemyPos;
        distance = new Vector3(Mathf.Abs(distance.x), Mathf.Abs(distance.y), 0f);
        bool xDistance = distance.x <= GameManager.Instance.globalConfig.invisibleCollision.x / 2;
        bool yDistance = distance.y <= GameManager.Instance.globalConfig.invisibleCollision.y / 2;

        return xDistance && yDistance;
    }

    private void EnemySpawn()
    {
        var amountToSpawn = GetSpawnQuantity();

        for (int i = 0; i < amountToSpawn; i++)
        {
            var enemy = GameManager.Instance.poolManager.GetEnemy();
            enemy.Spawn(GetSpawnPos());
            currentEnemyQuantitySpawned++;
            totalSpawned++;
            inLevelEnemies.Add(enemy);
            canSpawnEnemies = HasSpaceToSpawnEnemy();
            currentTime = UnityEngine.Random.Range(GameManager.Instance.globalConfig.minSpawnTime, GameManager.Instance.globalConfig.maxSpawnTime);

            if (!canSpawnEnemies) break;
        }
    }

    private int GetSpawnQuantity()
    {
        return UnityEngine.Random.Range(1, GameManager.Instance.globalConfig.MaxSpawnedAmount);
    }

    private Vector2 GetSpawnPos(bool extraRandomness = false)
    {
        var direction = GameManager.Instance.Player.Direction;
        var spawnPoint = GameManager.Instance.cameraController.GetSpawnPoint(direction, extraRandomness);
        return spawnPoint;
    }

    public void EnemyKilled(EnemyController enemyKilled)
    {
        GameManager.Instance.experienceSystem.AddExperience(enemyKilled.Model.BaseStats.experience);

        var mana = GameManager.Instance.poolManager.GetManaDrop();
        mana.transform.position = enemyKilled.transform.position;
        mana.Restart();

        inLevelEnemies.Remove(enemyKilled);
        totalKilled++;
        OnEnemyKilled.Invoke(totalKilled);
        currentEnemyQuantitySpawned--;
        canSpawnEnemies = HasSpaceToSpawnEnemy();
    }

    private bool HasSpaceToSpawnEnemy()
    {
        return currentEnemyQuantitySpawned < GameManager.Instance.globalConfig.maxEnemiesAtAllTimes;
    }

    private void OnDrawGizmosSelected()
    {
        if (GameManager.Instance.globalConfig.respawnEnemiesWhenTooFar)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(GameManager.Instance.Player.transform.position, new Vector3(GameManager.Instance.globalConfig.invisibleCollision.x, GameManager.Instance.globalConfig.invisibleCollision.y, 0f));
        }
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.updateManager.uncappedCustomUpdate.Remove(this);
    }
}
