using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState<T> : EnemyBaseState<T>
{
    private float currentTime;
    private Vector2 playerPos;

    public EnemyIdleState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

    }

    public override void Awake()
    {
        base.Awake();
        model.Idle();
    }

    public override void Execute()
    {
        if (model.CanMove(GameManager.Instance.Player.transform.position))
            Exit();
    }

    private void Exit()
    {
        onEndActivityCallback();
    }
}