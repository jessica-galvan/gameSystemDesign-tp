using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public struct EnemySpawnData : IWeight
{
    public string poolName;
    public EnemyController enemyPrefab;
    public int weight;
    [Tooltip("How many can be in the scene at the same time. If 0, there is no limit")]
    public int limitAmount;
    [Tooltip("How many can appear in the same run. If 0, there is no limit")]
    public int maxLimitPerRun;

    public bool CanBeSpawned => weight > 0;
    public bool HasMaxLimitRun => maxLimitPerRun > 0;
    public bool HasLimitAmount => limitAmount > 0;
    public int Weight => weight;
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
    private List<int> totalAmountSpawnedOfEachEnemy = new List<int>();
    private List<EnemySpawnData> currentSpawnables = new List<EnemySpawnData> { };

    public void Initialize()
    {
        canSpawnEnemies = true;
        GameManager.Instance.updateManager.uncappedCustomUpdate.Add(this);

        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            totalAmountSpawnedOfEachEnemy.Add(0);

            var enemySpawnData = GameManager.Instance.globalConfig.enemySpawnDataList[i];
            if (!enemySpawnData.CanBeSpawned) continue;
            currentSpawnables.Add(enemySpawnData);
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
        var enemyData = RandomWeight<EnemySpawnData>.Run(currentSpawnables, out var index);
        return EnemyTypeReturnes(enemyData.enemyPrefab); 
    }

    public int EnemyTypeReturnes(EnemyController enemy)
    {
        for (int i = 0; i < GameManager.Instance.globalConfig.enemySpawnDataList.Length; i++)
        {
            if (GameManager.Instance.globalConfig.enemySpawnDataList[i].enemyPrefab != enemy) continue;
            return i;
        }

        Debug.LogError($"EnemyType returns was an error.Cant find the {enemy.gameObject.name} in the array");
        return 0;
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
