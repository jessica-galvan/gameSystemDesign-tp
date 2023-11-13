using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IUpdate
{
    [ReadOnly] public int totalKilled = 0;
    [ReadOnly] public int totalSpawned = 0;
    [ReadOnly] public int currentEnemyQuantitySpawned = 0;
    [ReadOnly, SerializeField] private bool canSpawnEnemies;

    public Action<int> OnEnemyKilled = delegate { };
    private float currentTime;
    private List<EnemyController> inLevelEnemies = new List<EnemyController>();
    private List<int> totalAmountSpawnedOfEachEnemy = new List<int>();
    private List<EnemyController> currentSpawnables = new List<EnemyController> { };

    public void Initialize()
    {
        GameManager.Instance.experienceSystem.OnUpdateLevel += OnCharacterLevelUp;

        canSpawnEnemies = true;
        GameManager.Instance.updateManager.uncappedCustomUpdate.Add(this);

        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            totalAmountSpawnedOfEachEnemy.Add(0);

            var enemy = GameManager.Instance.globalConfig.enemySpawnDataList[i];
            if (enemy.Weight == 0) continue;
            currentSpawnables.Add(enemy);
        }
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
            var enemyIndex = GetEnemyTypeToSpawn();

            var enemy = GameManager.Instance.poolManager.GetEnemy(enemyIndex);
            enemy.Spawn(GetSpawnPos());

            RemoveIfReachedLimits(enemy);

            totalAmountSpawnedOfEachEnemy[enemyIndex]++;
            currentEnemyQuantitySpawned++;
            totalSpawned++;
            inLevelEnemies.Add(enemy);
            canSpawnEnemies = HasSpaceToSpawnEnemy();
            currentTime = UnityEngine.Random.Range(GameManager.Instance.globalConfig.minSpawnTime, GameManager.Instance.globalConfig.maxSpawnTime);

            if (!canSpawnEnemies) break;
        }
    }

    private int GetEnemyTypeToSpawn()
    {
        var enemyData = RandomWeight<EnemyController>.Run(currentSpawnables, out var index);
        return GetEnemyIndex(enemyData);
    }

    private void OnCharacterLevelUp(int currentLevel)
    {
        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            var enemy = GameManager.Instance.globalConfig.enemySpawnDataList[i];

            bool isGreaterMinimunLevel = enemy.EnemyData.MinLevel < 0  || currentLevel >= enemy.EnemyData.MinLevel;
            bool isLowerMaximumLevel = enemy.EnemyData.MaxLevel < 0 || currentLevel <= enemy.EnemyData.MaxLevel;
            bool isCurrentlyThere = currentSpawnables.Contains(enemy);

            bool shouldBeInside = isLowerMaximumLevel && isGreaterMinimunLevel;

            if (shouldBeInside && isCurrentlyThere) continue;

            if (shouldBeInside && !isCurrentlyThere)
                currentSpawnables.Add(enemy);
            else if (!shouldBeInside && isCurrentlyThere)
                currentSpawnables.Remove(enemy);
        }
    }

    private void RemoveIfReachedLimits(EnemyController enemy)
    {
        var index = GetEnemyIndex(enemy);

        if (enemy.EnemyData.HasMaxLimitRun)
        {
            if (totalAmountSpawnedOfEachEnemy[index] >= enemy.EnemyData.MaxLimitPerRun)
                currentSpawnables.Remove(enemy);
        }

        if (enemy.EnemyData.HasLimitAmount)
        {
            if (GameManager.Instance.poolManager.enemyPools[index].ActiveAmount >= enemy.EnemyData.LimitAmount)
                currentSpawnables.Remove(enemy);
        }
    }

    private void ReAddIfItCanBeAdded(EnemyController enemy)
    {
        if (enemy.EnemyData.HasMaxLimitRun) return;

        if (enemy.EnemyData.HasLimitAmount)
        {
            var index = GetEnemyIndex(enemy);
            if (GameManager.Instance.poolManager.enemyPools[index].ActiveAmount < enemy.EnemyData.LimitAmount)
                currentSpawnables.Add(enemy);
        }
    }

    public int GetEnemyIndex(EnemyController enemy)
    {
        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            if (enemy.EnemyData != GameManager.Instance.globalConfig.enemySpawnDataList[i].EnemyData) continue;
            return i;
        }

        return enemy.Index;
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
        GameManager.Instance.experienceSystem.AddExperience(enemyKilled.Model.BaseStats.Experience);

        var mana = GameManager.Instance.poolManager.GetManaDrop();
        mana.transform.position = enemyKilled.transform.position;
        mana.Restart();

        var enemyIndex = GetEnemyIndex(enemyKilled);

        ReAddIfItCanBeAdded(enemyKilled);

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
