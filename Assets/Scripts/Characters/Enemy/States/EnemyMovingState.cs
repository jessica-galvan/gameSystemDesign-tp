using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    public EnemyMovingState(T transitionInput) : base(transitionInput)
    {

    }

    public override void Awake()
    {
        base.Awake();
        controller.Model.OnBeingKocked += ChangeToIdle;
    }

    public override void Execute()
    {
        base.Execute();

        if (controller.Model.CanMove(GameManager.Instance.Player.transform.position))
            controller.Model.Move(controller.Model.pursuit.GetDir(controller.Model));
        else
            ChangeToIdle();
    }

    private void ChangeToIdle()
    {
        controller.SetState(EnemyStates.Idle);
    }

    public override void Sleep()
    {
        controller.Model.OnBeingKocked -= ChangeToIdle;
    }
}
