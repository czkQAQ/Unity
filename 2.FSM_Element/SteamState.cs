using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamState : BaseState
{
    

    public SteamState(Element fsm) : base(fsm) { }

    

    protected override void OnUpdate()
    {
        base.OnUpdate();
        FSM.rb.velocity = new Vector2(0, 2);

        FSM.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360, 360)));


    }


    //FSM.rb.velocity = new Vector2(0,1);
    //FSM.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360, 360)));  


    protected override void OnEnter()
    {
        FSM.SpriteRenderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 90 / 255f);

        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/water");

        float t = Random.Range(0.5f, 1f);
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.1f * t;

        FSM.Trigger.gameObject.tag = "steam";
        FSM.Collider.gameObject.layer = 28;
        FSM.SpriteRenderer.gameObject.layer = 28;
        FSM.Collider.gameObject.tag = "steam";

        // var effect = Resources.Load<GameObject>("Prefabs/Water_Lava/steam");
        // GameObject.Instantiate(effect, FSM.EffectPlace);

        FSM.radius = 0.07f;
    }

    protected override void OnTransition()
    {
        //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Type.STEAM);
        AudioManager.Instance.Play(AudioConstant.lavaonwater);
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)//中和毒气
    //{
    //    if (collision.tag == "Trap_Gas")
    //    {
    //        FSM.Transition(STATETYPE.Gas);
    //    }
    //}

    protected override void OnHitEnter(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {

            if (hit.tag == "Trap_Gas")
            {
                FSM.Transition(STATETYPE.Gas);
            }
        }
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ice")
        {
            FSM.Transition(STATETYPE.WATER);
        }
    }

}
