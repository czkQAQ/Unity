using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
//有限状态机
/*调用方法的逻辑：以OnTriggerEnter为例，挂载Element.cs的游戏物体发生碰撞时，触发系统类的OnTriggerEnter2D，然后调用状态接口的TriggerEnter2D，
接着调用BaseState的OnTriggerEnter2D，该方法又被子类的具体状态重写，所以最终调用的是具体状态类中的OnTriggerEnter2D*/


//状态枚举
public enum STATESTYPE { WATER, LAVA, STONE }

//系统类：负责状态的调用、切换和管理
public class Element : MonoBehaviour
{
    public STATESTYPE initStype = STATESTYPE.WATER;
    [SerializeField,ReadOnly]private STATESTYPE curStateType;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Collider2D Collider { get; set; }
    public Collider2D Trigger { get; set; }

    private Dictionary<STATESTYPE, IState> stateDic = new Dictionary<STATESTYPE, IState>();
    private IState curState;


    private void Awake()
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        Collider = transform.Find("Collider").GetComponent<CircleCollider2D>();
        Trigger = transform.Find("Trigger").GetComponent<CircleCollider2D>();

        stateDic.Add(STATESTYPE.WATER, new WaterState(this));
        stateDic.Add(STATESTYPE.LAVA, new LavaState(this));
        stateDic.Add(STATESTYPE.STONE, new StoneState(this));
    }

    private void Start()
    {
        Transition(initStype);
    }

    public void Transition(STATESTYPE targetState)
    {
        curStateType = targetState;
        curState = stateDic[curStateType];
        curState.Enter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        curState?.TriggerEnter2D(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        curState?.TriggerExit2D(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        curState?.TriggerStay2D(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        curState?.CollisionEnter2D(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        curState?.CollisionExit2D(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        curState?.CollisionStay2D(collision);
    }
}


//父类：具体状态的父类，在此实现所有接口，子类只需继承该类中的方法
public class BaseState : IState
{
    public Element FSM;
    public BaseState(Element fsm)
    {
        FSM = fsm;
    }

    //接口实现
    void IState.Enter()
    {
        OnEnter();
    }

    void IState.Transition()
    {
        OnTransition();
    }

    void IState.CollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    void IState.CollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2D(collision);
    }

    void IState.CollisionStay2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }

    void IState.TriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    void IState.TriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2D(collision);
    }

    void IState.TriggerStay2D(Collider2D collision)
    {
        OnTriggerStay2D(collision);
    }

    //继承
    protected virtual void OnEnter() { }
    protected virtual void OnTransition() { }
    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    protected virtual void OnCollisionStay2D(Collision2D collision) { }
    protected virtual void OnCollisionExit2D(Collision2D collision) { }
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    protected virtual void OnTriggerStay2D(Collider2D collsion) { }
    protected virtual void OnTriggerExit2D(Collider2D collsion) { }

}


//状态接口：定义状态的行为和切换
public interface IState
{
    void Enter();
    void Transition();
    void CollisionEnter2D(Collision2D collision);
    void CollisionStay2D(Collision2D collision);
    void CollisionExit2D(Collision2D collision);
    void TriggerEnter2D(Collider2D collision);
    void TriggerStay2D(Collider2D collision);
    void TriggerExit2D(Collider2D collision);
}
