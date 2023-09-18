using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : BaseCharacterModel
{
    public AttackDataSO attackStats;
    public Pursuit pursuit;

    public Action OnSpawned = delegate { };

    public override void Initialize()
    {
        base.Initialize();
        pursuit = new Pursuit();
    }

    public bool CanMove(Vector2 playerPos)
    {
        return GameManager.Instance.Player.Alive;
        //float distance = (playerPos - (Vector2) transform.position).magnitude;
        //return distance > baseStats.minDistanceFromPlayer;
    }

    public void Spawn(Vector2 spawnPoint)
    {
        transform.position = spawnPoint;
        gameObject.SetActive(true);
        OnSpawned.Invoke();
    }

    public void ResetStats()
    {
        LifeController.ResetStats();
    }
}
