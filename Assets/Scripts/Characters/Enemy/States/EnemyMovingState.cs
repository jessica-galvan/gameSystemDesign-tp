using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingState<T> : EnemyBaseState<T>
{
    public EnemyMovingState(T transitionInput, Action onEndActivityCallback) : base(transitionInput, onEndActivityCallback)
    {

    }

    public override void Execute()
    {
        base.Execute();

        if (model.CanMove(GameManager.Instance.Player.transform.position))
            model.Move(model.pursuit.GetDir(model));
        else
            Exit();
    }

    private void Exit()
    {
        onEndActivityCallback();
    }
}
