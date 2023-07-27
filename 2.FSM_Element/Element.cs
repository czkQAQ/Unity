using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Linq;
using UniRx;
using UnityEngine.Accessibility;

public enum STATETYPE { WATER, LAVA, STONE, CHARGEDWATER, STEAM, BrokenIce, Gas,Poison}
public class Element : MonoBehaviour
{
    public STATETYPE InitialState = STATETYPE.WATER;
    [SerializeField, ReadOnly] STATETYPE curStateType;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Transform EffectPlace { get; private set; }
    public CircleCollider2D Collider { get; set; }
    public CircleCollider2D Trigger { get; set; }
    private IState curState;
    private Dictionary<object, IState> stateDic = new Dictionary<object, IState>();

    public Rigidbody2D rb;

    public Transform eleTrans;
    public float radius;

    public bool inSwitch;
    public bool inWaterSwitch;
    public bool inPoisonSwitch;

    public float delayIceTime;

    public bool inPipe;
    public bool miniColl;//控制毒气的碰撞体变小以进入管道

    private void Awake()
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        EffectPlace = transform.Find("Effect");
        Collider = transform.Find("Collider").GetComponent<CircleCollider2D>();
        Trigger = transform.Find("Trigger").GetComponent<CircleCollider2D>();
        rb = transform.GetComponent<Rigidbody2D>();
        stateDic.Clear();

        eleTrans = Collider.gameObject.transform;

        AddState(STATETYPE.WATER, new WaterState(this));
        AddState(STATETYPE.LAVA, new LavaState(this));
        AddState(STATETYPE.STONE, new StoneState(this));
        AddState(STATETYPE.CHARGEDWATER, new ChargedWaterState(this));
        AddState(STATETYPE.STEAM, new SteamState(this));
        AddState(STATETYPE.BrokenIce, new BrokenIceState(this));
        AddState(STATETYPE.Gas, new GasState(this));
        AddState(STATETYPE.Poison, new PoisonState(this));

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        Transition(InitialState, true);
    }

    public void ClearEffect()
    {
        for (int i = 0; i < EffectPlace.childCount; i++)
        {
            GameObject.Destroy(EffectPlace.GetChild(i).gameObject);
        }
    }

    public void Transition(STATETYPE targetType, bool first = false)
    {
        if (inSwitch && targetType == STATETYPE.STONE) return;
        if (inPoisonSwitch && targetType == STATETYPE.CHARGEDWATER) return;
        if (inWaterSwitch && targetType == STATETYPE.CHARGEDWATER) return;

        curState?.Exit();
        ClearEffect();
        curStateType = targetType;
        curState = stateDic[curStateType];
        if (!first)
        {
            curState.Transition();
        }
        curState.Enter();
    }

    public void AddState(STATETYPE type, IState state)
    {
        state.Init();
        stateDic.Add(type, state);
    }

    public void SetDelayTrigger()
    {
        Trigger.enabled = false;
        Observable.Timer(System.TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
        {
            Trigger.enabled = true;
        });
    }

    #region Function
    

    bool isHit = false;
    private void Update()
    {
        curState?.Update();
        Collider2D[] hits = null;
        hits= Physics2D.OverlapCircleAll(transform.position, radius,-1);
        if(hits!=null&&hits.Length>0)
        {
            isHit = true;
            HitEnter(hits);
            HitStay(hits);
        }
        else
        {
            HitExit(hits);
            isHit = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        curState?.CollisionEnter2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        curState?.CollisionStay2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        curState?.CollisionExit2D(collision);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    curState?.TriggerEnter2D(collision);
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    curState?.TriggerStay2D(collision);
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    curState?.TriggerExit2D(collision);
    //}

    private void HitEnter(Collider2D[] hit)
    {
       

        curState?.HitEnter(hit);

        if (isHit) return;
    }

    private void HitStay(Collider2D[] hit)
    {
        curState?.HitStay(hit);
    }

    private void HitExit(Collider2D[] hit)
    {
        curState?.HitExit(hit);
    }
    #endregion
}

public class BaseState : IState
{
    public Element FSM { get; private set; }
    public BaseState(Element fsm)
    {
        FSM = fsm;
    }

    #region 接口实现

    void IState.Init()
    {
        OnInit();
    }
    void IState.Enter()
    {
        OnEnter();
    }

    void IState.Exit()
    {
        OnExit();
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

    //void IState.TriggerEnter2D(Collider2D collision)
    //{
    //    OnTriggerEnter2D(collision);
    //}

    //void IState.TriggerStay2D(Collider2D collision)
    //{
    //    OnTriggerStay2D(collision);
    //}

    //void IState.TriggerExit2D(Collider2D collision)
    //{
    //    OnTriggerExit2D(collision);
    //}

    void IState.HitEnter(Collider2D[] hits)
    {
        OnHitEnter(hits);
    }

    void IState.HitStay(Collider2D[] hits)
    {
        OnHitStay(hits);
    }
    
    void IState.HitExit(Collider2D[] hits)
    {
        OnHitExit(hits);
    }

    void IState.Update()
    {
        OnUpdate();
    }

    #endregion

    #region 继承
    protected virtual void OnInit() { }
    protected virtual void OnEnter() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnExit() { }
    protected virtual void OnTransition() { }
    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    protected virtual void OnCollisionStay2D(Collision2D collision) { }
    protected virtual void OnCollisionExit2D(Collision2D collision) { }

    //protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    //protected virtual void OnTriggerStay2D(Collider2D collision) { }
    //protected virtual void OnTriggerExit2D(Collider2D collision) { }

    protected virtual void OnHitEnter(Collider2D[] hits) { }
    protected virtual void OnHitStay(Collider2D[] hits) { }
    protected virtual void OnHitExit(Collider2D[] hits) { }
    #endregion

}

public interface IState
{
    Element FSM { get; }
    void Init();
    void Enter();
    void Transition();
    void CollisionEnter2D(Collision2D collision);
    void CollisionStay2D(Collision2D collision);
    void CollisionExit2D(Collision2D collision);
    //void TriggerEnter2D(Collider2D collision);
    //void TriggerStay2D(Collider2D collision);
    //void TriggerExit2D(Collider2D collision);

    void HitEnter(Collider2D[] hits);
    void HitStay(Collider2D[] hits);
    void HitExit(Collider2D[] hits);

    void Update();
    void Exit();
}