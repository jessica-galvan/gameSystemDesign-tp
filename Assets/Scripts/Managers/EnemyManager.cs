using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IUpdate
{
    [ReadOnly] public int totalKilled = 0;
    [ReadOnly] public int totalSpawned = 0;
    [ReadOnly] public int currentEnemyQuantitySpawned = 0;

    public Action<int> OnEnemyKilled = delegate { };
    private bool canSpawnEnemies;
    private float currentTime;
    private HashSet<EnemyController> inLevelEnemies = new HashSet<EnemyController>();

    public void Initialize()
    {
        canSpawnEnemies = true;

        GameManager.Instance.updateManager.fixCustomUpdater.Add(this);
    }

    public void Refresh()
    {
        if (!GameManager.Instance.CanUpdate) return;

        if (canSpawnEnemies)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
                EnemySpawn();
        }
    }

    private void EnemySpawn()
    {
        var enemy = Instantiate(GameManager.Instance.prefabReferences.enemyPrefab);
        enemy.transform.position = GetSpawnPos();
        enemy.Initialize();
        currentEnemyQuantitySpawned++;
        totalSpawned++;
        inLevelEnemies.Add(enemy);
        currentTime = UnityEngine.Random.Range(2f, 5f);
    }

    private Vector2 GetSpawnPos()
    {
        //TODO get direction player is going!!!
        return GameManager.Instance.playerSpawningPoint.position;
    }

    public void EnemyKilled(EnemyController enemyKilled)
    {
        inLevelEnemies.Remove(enemyKilled);
        totalKilled++;
        OnEnemyKilled.Invoke(totalKilled);
        currentEnemyQuantitySpawned--;
        canSpawnEnemies = HasSpaceToSpawnEnemy(); //is it lazy computation if we only update it when the number changes instead of doing the method on every frame of the DoUpdate?)
    }

    private bool HasSpaceToSpawnEnemy()
    {
        return currentEnemyQuantitySpawned < GameManager.Instance.globalConfig.maxEnemiesAtAllTimes;
    }

    private void OnDestroy()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.updateManager.fixCustomUpdater.Remove(this);
    }
}
