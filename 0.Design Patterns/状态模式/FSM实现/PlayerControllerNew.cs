using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    状态模式
    有限状态机的实现。
 */


//状态枚举
//public enum STATE { StandState, JumpState, CrouchState }

//系统类
public class PlayerControllerNew : MonoBehaviour
{
    private IState curState;
    [SerializeField]private STATE curStateType;
    private Dictionary<STATE, IState> stateDic = new Dictionary<STATE, IState>();

    private void Awake()
    {
        stateDic.Add(STATE.StandState, new StandState(this));
        stateDic.Add(STATE.JumpState, new JumpState(this));
        stateDic.Add(STATE.CrouchState, new CrouchState(this));

        SetState(STATE.StandState);
    }

    private void Update()
    {
        curState?.OnUptate();
    }


    public void SetState(STATE targetState)
    {
        curStateType = targetState;
        curState = stateDic[curStateType];
    }

}


//父类
public class BaseState : IState
{
    public PlayerControllerNew FSM;
    public BaseState(PlayerControllerNew fsm)
    {
        FSM = fsm;
    }

    //接口实现
    void IState.OnHandleInput()
    {
        HandleInput();
    }

    void IState.OnUptate()
    {
        Updete();
    }

    //继承
    protected virtual void HandleInput() { }
    protected virtual void Updete() { }
}

//状态接口
public interface IState
{
    void OnHandleInput();
    void OnUptate();
}