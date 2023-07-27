using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class WaterState : BaseState
{
    

    public WaterState(Element fsm) : base(fsm) { }

    //bool canChangeIce;


    protected override void OnEnter()
    {
        //canChangeIce = false;
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/dropTexture");
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.035f;

        FSM.SpriteRenderer.color = new Color(0 / 255f, 191 / 255f, 255 / 255f, 130f / 255);
        var effect = Resources.Load<GameObject>("EFFECT Tuyet/Prefab/Rescue Hero/water");
        GameObject.Instantiate(effect, FSM.EffectPlace);
        effect.transform.localPosition = Vector3.zero;

        //FSM.Trigger.tag = "Water";
        FSM.SpriteRenderer.gameObject.layer = 4;
        FSM.Collider.gameObject.layer = 4;
        FSM.Collider.gameObject.tag = "Water";

        FSM.radius = 0.07f;
        //Observable.Timer(System.TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        //{
        //    canChangeIce = true;
        //});
        infire = false;
    }

    protected override void OnTransition()
    {
        //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Type.WATER);
        AudioManager.Instance.Play(AudioConstant.water_drop);
    }

    private bool infire;

    #region 注释
    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    if (collision.tag == "Trap_Lava")
    //    {
    //        FSM.Transition(STATETYPE.STONE);
    //    }
    //    else if (collision.tag == "Charged")
    //    {
    //        FSM.Transition(STATETYPE.CHARGEDWATER);
    //    }
    //    else if (collision.tag == "Tag_Stone" && !collision.gameObject.name.Contains("FallingStone"))
    //    {
    //        FSM.Transition(STATETYPE.STONE);
    //    }
    //    else if (collision.tag == "fire")
    //    {
    //        FSM.StartCoroutine(DelayToChange2Steam());
    //    }
    //    else if (collision.tag == "Ice" || collision.tag == "BrokenIce")
    //    {
    //        //FSM.StartCoroutine(DelayToChange2Ice());
    //        if(!infire){
    //        FSM.Transition(STATETYPE.BrokenIce);
    //        }
    //    }

    //}

    //protected override void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.tag == "BrokenIce")
    //    {
    //        FSM.StartCoroutine(DelayToChange2Ice());
    //        //FSM.Transition(STATETYPE.BrokenIce);
    //    }

    //    if(other.tag == "fire"){
    //        infire = true;
    //    }
    //}、
    #endregion


    protected override void OnUpdate()
    {
        base.OnUpdate();
    }



    protected override void OnHitEnter(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {
            if (hit.tag== "Trap_Lava")
            {
                //if (PoisonSwitch.inSwitch == true) return;
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if(LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            else if ( hit.tag == "Charged")
            {
                
                //if (LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.CHARGEDWATER);
            }
            else if (hit.tag == "Tag_Stone" && hit.gameObject.name != "FallingStone")
            {
                //if (PoisonSwitch.inSwitch == true) return;
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            else if (hit.tag == "fire")
            {
                FSM.StartCoroutine(DelayToChange2Steam());
            }
            //else if (hit.tag == "Ice" ||hit.tag == "BrokenIce")
            //{
            //    if (!infire&&canChangeIce)
            //    {
            //        stayIce = true;
            //        //FSM.Transition(STATETYPE.BrokenIce);
            //        FSM.StartCoroutine(DelayToChange2Ice());
            //    }
            //}
            else if(hit.tag == "Trap_Gas")
            {
                FSM.Transition(STATETYPE.Poison);
            }else if ((hit.tag == "Ice" || hit.tag == "BrokenIce") && FSM.delayIceTime == 0)
            {
                FSM.Transition(STATETYPE.BrokenIce);
            }
        }

    }

    protected override void OnHitStay(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {
            //if(hit.tag == "BrokenIce")
            //{
            //    FSM.StartCoroutine(DelayToChange2Ice());
            //}
            if(hit.tag == "fire")
            {
                infire = true;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Ice" || collision.collider.gameObject.tag == "BrokenIce")
        {
            stayIce = true;
            FSM.StartCoroutine(DelayToChange2Ice());
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Ice" || collision.collider.gameObject.tag == "BrokenIce")
        {
            stayIce = true;
            FSM.StartCoroutine(DelayToChange2Ice());
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Ice" || collision.collider.gameObject.tag == "BrokenIce")
        {
            stayIce = false;
        }
    }

    IEnumerator DelayToChange2Steam()
    {
        float t = Random.Range(1f, 2f);
        Debug.Log(t);
        yield return new WaitForSeconds(t);

        if (FSM.Collider.gameObject.layer == 4)
        {
            FSM.Transition(STATETYPE.STEAM);
            Debug.Log("change2Steam");
        }
    }

    private bool stayIce;
    IEnumerator DelayToChange2Ice()
    {

    yield return new WaitForSeconds(FSM.delayIceTime);
    if(!infire && stayIce){
    FSM.Transition(STATETYPE.BrokenIce);
    Debug.Log("change2Ice");
    }
    }
}
