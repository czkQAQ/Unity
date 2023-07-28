using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    状态模式：类的行为基于它的状态而改变。
    简单实现。
 */


//状态枚举
public enum STATE { StandState,JumpState,CrouchState}

public class PlayerController : MonoBehaviour
{
    private STATE curState;//当前状态

    private void Awake()
    {
        //初始为站立状态
        curState = STATE.StandState;
    }


    //修改当前状态
    public void SetState(STATE targetState)
    {
        curState = targetState;
    }

    private void Update()
    {
        //监听输入
        HandleInput();
    }

    private void HandleInput()
    {
        switch (curState)
        {
            //玩家在站立状态可以——>跳跃、下蹲
            case STATE.StandState:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    SetState(STATE.JumpState);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SetState(STATE.CrouchState);
                }
                break;
            //玩家在跳跃状态可以——>站立
            case STATE.JumpState:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SetState(STATE.StandState);
                }
                break;
            //玩家在下蹲状态可以——>站立
            case STATE.CrouchState:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    SetState(STATE.StandState);
                }
                break;
            default:
                break;
        }
    }

}
