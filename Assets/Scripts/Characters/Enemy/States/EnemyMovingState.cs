using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    public EnemyMovingState(T transitionInput) : base(transitionInput) { }

    public override void Awake() { }

    public override void Execute()
    { 
        if(!controller.Model.CanMove(GameManager.Instance.Player.transform.position))
            ChangeToIdle();
    }

    public override void FixedExecute()
    {
        controller.Model.Move(controller.Model.pursuit.GetDir(controller.Model));
    }

    private void ChangeToIdle()
    {
        controller.SetState(EnemyStates.Idle);
    }

    public override void Sleep() { }
}
