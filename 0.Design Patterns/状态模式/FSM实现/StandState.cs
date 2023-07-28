using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandState : BaseState
{
    public StandState(PlayerControllerNew fsm) : base(fsm) { }

    protected override void Updete()
    {
        HandleInput();
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FSM.SetState(STATE.JumpState);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FSM.SetState(STATE.CrouchState);
        }
    }
}
