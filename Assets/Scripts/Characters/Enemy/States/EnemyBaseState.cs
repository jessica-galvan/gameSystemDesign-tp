using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState<T> : State<T>
{
    protected EnemyController controller;
    protected FSM<T> fsm;
    protected T transitionInput;

    public EnemyBaseState(T transitionInput)
    {
        this.transitionInput = transitionInput;
    }

    public void InitializeState(EnemyController controller, FSM<T> stateFSM)
    {
        this.controller = controller;
        fsm = stateFSM;
    }
}
