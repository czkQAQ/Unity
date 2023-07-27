using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SWITCHTYPE {WATER,LAVA,CHARGEDWATER,POISON}

public class ElementSwitch : MonoBehaviour
{
    public SWITCHTYPE InitialStype = SWITCHTYPE.WATER;
    [SerializeField, ReadOnly] SWITCHTYPE curSwitchType;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public BoxCollider2D Trigger { get; set; }
    private ISwitch curSwitch;
    private Dictionary<object, ISwitch> switchDic = new Dictionary<object, ISwitch>();

    private Vector2 centerPos;
    public float radius;
    public LayerMask lyElement;

    public float rotate;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        Trigger = transform.Find("Trigger").GetComponent<BoxCollider2D>();

        switchDic.Add(SWITCHTYPE.WATER, new WaterSwitch(this));
        switchDic.Add(SWITCHTYPE.LAVA, new LavaSwitch(this));
        switchDic.Add(SWITCHTYPE.CHARGEDWATER, new ChargedWaterSwitch(this));
        switchDic.Add(SWITCHTYPE.POISON, new PoisonSwitch(this));

        centerPos = transform.position;

        rotate = this.transform.eulerAngles.z;
    }

    private void Start()
    {
        Transition(InitialStype);
    }

    public bool noStay;
    private void Update()
    {
        curSwitch?.Update();
        var hitElement = Physics2D.OverlapCircle(centerPos, radius, lyElement);
        if(hitElement == null ||
            (hitElement.gameObject.tag != "Water" && hitElement.gameObject.tag != "Trap_Gas" && hitElement.gameObject.tag != "Charged" && hitElement.gameObject.tag != "Trap_Lava"))
        {
            noStay = true;
        }
        else
        {
            noStay = false;
        }
    }

    public void Transition(SWITCHTYPE targetType)
    {
        curSwitch?.Exit();
        curSwitchType = targetType;
        curSwitch = switchDic[curSwitchType];
        curSwitch.Enter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        curSwitch?.TriggerEnter2D(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        curSwitch?.TriggerStay2D(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        curSwitch?.TriggerExit2D(collision);
    }
}

public class BaseSwitch : ISwitch
{
    public ElementSwitch FSM { get; private set; }
    public BaseSwitch(ElementSwitch fsm)
    {
        FSM = fsm;
    }


    //接口实现
    void ISwitch.Enter()
    {
        OnEnter();
    }
    void ISwitch.TriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
    void ISwitch.TriggerStay2D(Collider2D collision)
    {
        OnTriggerStay2D(collision);
    }
    void ISwitch.TriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2D(collision);
    }
    void ISwitch.Exit()
    {
        OnExit();
    }

    void ISwitch.Update()
    {
        OnUpdate();
    }

    //继承
    protected virtual void OnEnter() { }
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    protected virtual void OnTriggerStay2D(Collider2D collision) { }
    protected virtual void OnTriggerExit2D(Collider2D collision) { }
    protected virtual void OnExit() { }

    protected virtual void OnUpdate() { }
}

//接口
public interface ISwitch{
    ElementSwitch FSM{ get; }

    void Enter();
    void TriggerEnter2D(Collider2D collision);
    void TriggerStay2D(Collider2D collision);
    void TriggerExit2D(Collider2D collision);
    void Exit();

    void Update();
}
