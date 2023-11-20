using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : BaseCharacterModel, IDamage
{
    public AttackDataSO attackStats;
    public Pursuit pursuit;

    public Action OnSpawned = delegate { };

    public AttackDataSO AttackData => attackStats;

    public override void Initialize(CharacterDataSO stats)
    {
        base.Initialize(stats);
        attackStats.Initialize();
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
        SetVisualTakeDamage(false);
    }


    public override void TakeDamage(int damage, bool ignoreCooldown = true)
    {
        if (!Alive) return;
        if (!CanTakeDamage && !ignoreCooldown) return;

        base.TakeDamage(damage, ignoreCooldown);

        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.enemyTakeDamageSound);
    }

    public override void TakeDamage(int damage, Vector2 knockback,  bool ignoreCooldown = true)
    {
        if (!Alive) return;
        if (!CanTakeDamage && !ignoreCooldown) return;

        base.TakeDamage(damage, knockback, ignoreCooldown);

        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.enemyTakeDamageSound);
    }
}
