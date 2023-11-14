using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStage
{
    [Tooltip("The range is between 0 and 1, taking into account the per")]
    [Range(0f, 1f)] public float startTime = 0f;
    public List<EnemyData> allEnemyData;
    public int maxSpawnedAtTheSameTime;
    public int maxTotalEnemies;
    public float minSpawnWaitTime = 0.5f;
    public float maxSpawnWaitTime = 1f;

    private List<int> currentSpawnedAmountOfEachEnemyInStage = new List<int>();

    public void Initialize()
    {
        for (int i = 0; i < allEnemyData.Count; i++)
            currentSpawnedAmountOfEachEnemyInStage.Add(0);
    }

    public void AddToCounter(EnemyData enemyData, int amount)
    {
        currentSpawnedAmountOfEachEnemyInStage[enemyData.EnemyID] += amount;
    }

    public bool CanSpawnEnemy(EnemyData enemyData, int currentAmount)
    {
        if (enemyData.HasMaxLimitRun)
            return currentSpawnedAmountOfEachEnemyInStage[enemyData.EnemyID] <= enemyData.maxLimitPerStage;

        if (enemyData.HasLimitAmount)
            return currentAmount <= enemyData.limitAmountAtSameTime;

        return true;
    }
}

[Serializable]
public class EnemyData : IWeight
{
    public EnemyController enemyController;
    public int EnemyID => enemyController.ID;

    public int weight;
    [Tooltip("How many can be in the scene at the same time. If 0, there is no limit")]
    public int limitAmountAtSameTime;
    [Tooltip("How many can appear in the same run. If 0, there is no limit")]
    public int maxLimitPerStage;

    [Header("Difficulty Scalers")]
    [Tooltip("If you want the enemy gain more hp, change the hp multiplier. can be used to increase or decrease. Default: -1 so it doesn't multiply. Will Reset to base life before applying multiplied")]
    public float healthMultiplier = -1;
    [Tooltip("Same as Health Multiplier")]
    public float speedMultiplier = -1;
    [Tooltip("Same as Health Multiplier")]
    public float experienceMultiplier = -1;

    public int Weight => weight;
    public bool HasMaxLimitRun => maxLimitPerStage > 0;
    public bool HasLimitAmount => limitAmountAtSameTime > 0;

    public void ScaleUpDifficulty(CharacterDataSO data)
    {
        data.Reset(healthMultiplier > 0, experienceMultiplier > 0, speedMultiplier > 0);

        if (healthMultiplier > 0)
            data.ChangeMaxLife(Mathf.RoundToInt(data.MaxLife + data.MaxLife * healthMultiplier));

        if (speedMultiplier > 0)
            data.ChangeMaxLife(Mathf.RoundToInt(data.MovementSpeed + data.MovementSpeed * speedMultiplier));

        if (experienceMultiplier > 0)
            data.ChangeMaxLife(Mathf.RoundToInt(data.Experience + data.Experience * experienceMultiplier));
    }
}

public class EnemyManager : MonoBehaviour, IUpdate
{
    [Header("Info")]
    [ReadOnly, SerializeField] private int totalKilled = 0;
    [ReadOnly, SerializeField] private int totalSpawned = 0;
    [ReadOnly, SerializeField] private int currentEnemyQuantitySpawned = 0;
    [ReadOnly, SerializeField] private bool canSpawnEnemies;
    [ReadOnly, SerializeField] private float stageTimerT = 0f;

    public Action<int> OnEnemyKilled = delegate { };
    private float currentTime;
    private List<EnemyController> inLevelEnemies = new List<EnemyController>();
    private List<int> totalAmountSpawnedOfEachEnemy = new List<int>();
    private List<EnemyData> currentSpawnables = new List<EnemyData> { };

    private int currentStageIndex = 0;
    private float nextStageTime = 0;
    private EnemyStage currentStage;
    private bool isOverTime = false;

    public void Initialize()
    {
        canSpawnEnemies = true;
        GameManager.Instance.updateManager.uncappedCustomUpdate.Add(this);

        for (int i = 0; i < GameManager.Instance.globalConfig.allStages.Length; i++)
            totalAmountSpawnedOfEachEnemy.Add(0);

        ChangeStage(0);
    }

    private void ChangeStage(int currentIndex)
    {
        if(currentIndex < GameManager.Instance.globalConfig.allStages.Length)
        {
            print($"Current Stage: {currentIndex}");

            currentStage = GameManager.Instance.globalConfig.allStages[currentIndex];
            currentStageIndex = currentIndex;
            isOverTime = false;

            if ((currentIndex + 1) < GameManager.Instance.globalConfig.allStages.Length)
                nextStageTime = GameManager.Instance.globalConfig.allStages[currentIndex + 1].startTime;
            else
                nextStageTime = 1f;

        }
        else
        {
            print($"Current Stage: Last Stage");
            isOverTime = true;
            currentStage = GameManager.Instance.globalConfig.lastStage;
        }

        RefreshSpawnableList();
    }

    public void Refresh(float deltaTime)
    {
        if (!GameManager.Instance.CanUpdate) return;

        if (!isOverTime)
        {
            stageTimerT += deltaTime / GameManager.Instance.globalConfig.gameplayDuration;
            if (stageTimerT >= nextStageTime)
            {
                currentStageIndex++;
                ChangeStage(currentStageIndex);
            }
        }

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
            if (!TryGetEnemyTypeToSpawn(out int enemyIndex)) break;

            var enemy = GameManager.Instance.poolManager.GetEnemy(enemyIndex);
            enemy.Spawn(GetSpawnPos());

            RefreshSpawnableList();

            totalAmountSpawnedOfEachEnemy[enemyIndex]++;
            currentEnemyQuantitySpawned++;
            totalSpawned++;
            inLevelEnemies.Add(enemy);
            canSpawnEnemies = HasSpaceToSpawnEnemy();
            currentTime = UnityEngine.Random.Range(currentStage.minSpawnWaitTime, currentStage.maxSpawnWaitTime);

            if (!canSpawnEnemies) break;
        }
    }

    private bool TryGetEnemyTypeToSpawn(out int ID)
    {
        ID = 0;
        if (currentSpawnables.Count == 0) return false;
        var enemyData = RandomWeight<EnemyData>.Run(currentSpawnables, out var index);
        ID = enemyData.EnemyID;
        return true;
    }

    private void RefreshSpawnableList()
    {
        currentSpawnables.Clear();
        for (int i = 0; i < currentStage.allEnemyData.Count; i++)
        {
            if (GameManager.Instance.poolManager.enemyPools.TryGetValue(currentStage.allEnemyData[i].EnemyID, out var pool))
            {
                if (currentStage.CanSpawnEnemy(currentStage.allEnemyData[i], pool.ActiveAmount))
                    currentSpawnables.Add(currentStage.allEnemyData[i]);
            }
        }
    }

    private int GetSpawnQuantity()
    {
        return UnityEngine.Random.Range(1, currentStage.maxSpawnedAtTheSameTime);
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

        RefreshSpawnableList();

        inLevelEnemies.Remove(enemyKilled);
        totalKilled++;
        OnEnemyKilled.Invoke(totalKilled);
        currentEnemyQuantitySpawned--;
        canSpawnEnemies = HasSpaceToSpawnEnemy();
    }

    private bool HasSpaceToSpawnEnemy()
    {
        return currentEnemyQuantitySpawned < currentStage.maxTotalEnemies;
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
