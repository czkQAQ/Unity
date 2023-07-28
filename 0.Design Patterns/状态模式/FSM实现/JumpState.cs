using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerControllerNew fsm) : base(fsm) { }

    protected override void Updete()
    {
        HandleInput();
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FSM.SetState(STATE.StandState);
        }
    }
}
