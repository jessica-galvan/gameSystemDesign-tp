using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyIdleState<T> : EnemyBaseState<T>
{
    public EnemyIdleState(T transitionInput) : base(transitionInput) { }

    public override void Awake()
    {
        controller.Model.Idle();
    }

    public override void Execute()
    {
        if (controller.Model.CanMove(GameManager.Instance.Player.transform.position))
            controller.SetState(EnemyStates.Move);
    }
    public override void Sleep() { }
}