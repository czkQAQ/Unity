using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ״̬ģʽ�������Ϊ��������״̬���ı䡣
    ��ʵ�֡�
 */


//״̬ö��
public enum STATE { StandState,JumpState,CrouchState}

public class PlayerController : MonoBehaviour
{
    private STATE curState;//��ǰ״̬

    private void Awake()
    {
        //��ʼΪվ��״̬
        curState = STATE.StandState;
    }


    //�޸ĵ�ǰ״̬
    public void SetState(STATE targetState)
    {
        curState = targetState;
    }

    private void Update()
    {
        //��������
        HandleInput();
    }

    private void HandleInput()
    {
        switch (curState)
        {
            //�����վ��״̬���ԡ���>��Ծ���¶�
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
            //�������Ծ״̬���ԡ���>վ��
            case STATE.JumpState:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SetState(STATE.StandState);
                }
                break;
            //������¶�״̬���ԡ���>վ��
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
