using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : BaseState
{
    public CrouchState(PlayerControllerNew fsm) : base(fsm) { }

    protected override void Updete()
    {
        HandleInput();
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FSM.SetState(STATE.StandState);
        }
    }
}
