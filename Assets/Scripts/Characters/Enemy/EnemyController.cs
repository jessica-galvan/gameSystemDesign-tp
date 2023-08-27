using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStates
{
    Idle,
    Move,
}

public class EnemyController : BaseCharacterController
{
    [ReadOnly, SerializeField] private EnemyStates currentEnemyState;

    private FSM<EnemyStates> fsm;
    public EnemyModel Model { get; private set; }


    public override void Initialize()
    {
        base.Initialize();
        Model = GetComponent<EnemyModel>();
        Model.Initialize();
        InitializeFSM();
    }

    public void InitializeFSM()
    {
        fsm = new FSM<EnemyStates>();

        var idle = new EnemyIdleState<EnemyStates>(EnemyStates.Move, () => SetState(EnemyStates.Move));
        var move = new EnemyMovingState<EnemyStates>(EnemyStates.Idle, () => SetState(EnemyStates.Idle));

        idle.InitializeState(Model, fsm);
        move.InitializeState(Model, fsm);

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
        if (!GameManager.Instance.CanUpdate) return;
        fsm.OnUpdate();
    }

    protected override void AddToUpdate()
    {

    }

    protected override void RemoveFromUpdate()
    {

    }
}
