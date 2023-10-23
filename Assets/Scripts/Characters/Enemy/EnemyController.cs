using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStates
{
    Idle,
    Move,
}

public class EnemyController : BaseCharacterController<EnemyModel>, IPoolable
{
    [ReadOnly, SerializeField] private EnemyStates currentEnemyState;

    private FSM<EnemyStates> fsm;

    public override void Initialize()
    {
        base.Initialize();
        Model.LifeController.OnDeath += OnDie;
        InitializeFSM();
    }

    public void InitializeFSM()
    {
        fsm = new FSM<EnemyStates>();

        var idle = new EnemyIdleState<EnemyStates>(EnemyStates.Move);
        var move = new EnemyMovingState<EnemyStates>(EnemyStates.Idle);

        idle.InitializeState(this, fsm);
        move.InitializeState(this, fsm);

        idle.AddTransition(EnemyStates.Move, move);
        move.AddTransition(EnemyStates.Idle, idle);

        currentEnemyState = EnemyStates.Idle;
        fsm.SetInit(idle); //until enemy is spawn, better left them on idle
    }

    public void SetState(EnemyStates newState)
    {
        if (currentEnemyState != newState)
        {
            currentEnemyState = newState;
            fsm.Transition(newState);
        }
    }

    public override void Refresh()
    {
        if (!CanUpdate()) return;
        fsm.OnUpdate();
    }

    public override void FixedRefresh()
    {
        if (!CanUpdate()) return;
        fsm.OnFixedUpdate();
    }

    protected override void AddToUpdate()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
        GameManager.Instance.updateManager.AddFixedUpdate(this);
    }

    protected override void RemoveFromUpdate()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        GameManager.Instance.updateManager.RemoveFixedUpdate(this);
    }

    private void OnDie()
    {
        var deathVFX = GameManager.Instance.poolManager.GetParticle(ParticleEffect.ParticleType.Death);
        deathVFX.Set(Model.transform);

        GameManager.Instance.poolManager.ReturnEnemy(this);
        GameManager.Instance.enemyManager.EnemyKilled(this);
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        RemoveFromUpdate();
    }

    public void Spawn(Vector2 position)
    {
        Model.Spawn(position);
        Model.ResetStats();
        gameObject.SetActive(true);
        AddToUpdate();
    }
}
