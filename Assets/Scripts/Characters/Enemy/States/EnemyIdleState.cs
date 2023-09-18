using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState<T> : EnemyBaseState<T>
{
    public EnemyIdleState(T transitionInput) : base(transitionInput)
    {

    }

    public override void Awake()
    {
        base.Awake();
        controller.Model.Idle();

        if (controller.Model.wasKnocked)
            controller.Model.currentKnockbackTimer = controller.Model.BaseStats.knockbackRecovery;
    }

    public override void Execute()
    {
        if (controller.Model.wasKnocked)
        {
            controller.Model.UpdateKnockbackTimer();
            return;
        }

        if (controller.Model.CanMove(GameManager.Instance.Player.transform.position))
            controller.SetState(EnemyStates.Move);
    }
    public override void Sleep()
    {
        controller.Model.wasKnocked = false;
    }
}