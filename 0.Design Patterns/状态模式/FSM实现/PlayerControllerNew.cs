using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ״̬ģʽ
    ����״̬����ʵ�֡�
 */


//״̬ö��
//public enum STATE { StandState, JumpState, CrouchState }

//ϵͳ��
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


//����
public class BaseState : IState
{
    public PlayerControllerNew FSM;
    public BaseState(PlayerControllerNew fsm)
    {
        FSM = fsm;
    }

    //�ӿ�ʵ��
    void IState.OnHandleInput()
    {
        HandleInput();
    }

    void IState.OnUptate()
    {
        Updete();
    }

    //�̳�
    protected virtual void HandleInput() { }
    protected virtual void Updete() { }
}

//״̬�ӿ�
public interface IState
{
    void OnHandleInput();
    void OnUptate();
}