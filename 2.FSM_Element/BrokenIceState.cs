using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenIceState : BaseState
{
    
    public BrokenIceState(Element fsm) : base(fsm) { }

    protected override void OnInit()
    {
        if (FSM.InitialState == STATETYPE.BrokenIce)
        {
            AudioManager.Instance.Play(AudioConstant.rock1);
        }

    }

    protected override void OnEnter()
    {
        Sprite spr = Resources.Load<Sprite>("Images/InGame/Game_objects/Trap/Ice_Stone");
        if(spr != null){
            FSM.SpriteRenderer.sprite = spr;
            FSM.Trigger.gameObject.tag = "BrokenIce";
            FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;//图片大小
            FSM.SpriteRenderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }
        else{
            Debug.Log("can not get sprite for give url");
        }
        FSM.Trigger.gameObject.tag = "BrokenIce";
         FSM.Collider.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics Materials/Regular Water.physicsMaterial2D");
        FSM.Collider.gameObject.layer = 29;
        FSM.Collider.gameObject.tag = "BrokenIce";

        FSM.radius = 0.07f;
    }

    protected override void OnTransition()
    {
        //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Type.BROKENICE);
        AudioManager.Instance.Play(AudioConstant.rock1);
    }

    //protected override void OnTriggerEnter2D(Collider2D collision) {
    //    if(collision.tag == "fire"){
    //        FSM.StartCoroutine(DelayChange2Water());
    //    }
    //    if(collision.tag == "Trap_Lava" || (collision.tag == "Tag_Stone" && !collision.gameObject.name.Contains("FallingStone"))){
    //        FSM.Transition(STATETYPE.STONE);
    //    }

    //}

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnHitEnter(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {
            if (hit.tag == "fire")
            {
                FSM.StartCoroutine(DelayChange2Water());
            }
            if (hit.tag == "Trap_Lava" || (hit.tag == "Tag_Stone" && !hit.gameObject.name.Contains("FallingStone")))
            {
                FSM.Transition(STATETYPE.STONE);
            }
        }
    }

    IEnumerator DelayChange2Water(){
        FSM.Trigger.enabled = false;
        yield return new WaitForSeconds(1f);
        FSM.Trigger.enabled = true;
        FSM.Transition(STATETYPE.WATER);
        Debug.Log("change2Water");
    }
}
