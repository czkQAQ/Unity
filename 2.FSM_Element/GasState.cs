using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class GasState : BaseState
{
    float timeFly = 0;
    float speedMove = 1;
    float colliderRadius;
    float triggerRadius;
    public GasState(Element fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.color = new Color(123 / 255f, 255 / 255f, 117 / 255f, 90 / 255f);
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/dropTexture");
        colliderRadius = FSM.Collider.radius;
        triggerRadius = FSM.Trigger.radius;
        if (FSM.miniColl)
        {
            FSM.Collider.radius = 0.05f;
        }
        else
        {
            FSM.Collider.radius = 0.2f;
        }
        
        FSM.Trigger.radius = 0.25f;
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.1f;
        FSM.rb.gravityScale = 0;
        FSM.SpriteRenderer.gameObject.layer = 24;

        if (FSM.miniColl)
        {
            FSM.Collider.gameObject.layer = 0;
        }
        else
        {
            FSM.Collider.gameObject.layer = 24;
        }
        
        FSM.Collider.gameObject.tag = "Trap_Gas";

        FSM.radius = 0.07f;
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "BodyPlayer")
    //    {
    //        if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
    //        {
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
    //            {
    //                PlayerManager.Instance.OnPlayerDie(true);
    //            }
    //        }
    //    }
    //}

    protected override void OnHitEnter(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {
            if (hit.gameObject.tag == "BodyPlayer")
            {
                if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
                {
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        PlayerManager.Instance.OnPlayerDie(true);
                    }
                }
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ice")
        {
            FSM.Transition(STATETYPE.Poison);
        }
    }


    protected override void OnExit()
    {
        FSM.Collider.radius = colliderRadius;
        FSM.Trigger.radius = triggerRadius;

        FSM.rb.velocity = Vector2.zero;
        FSM.rb.gravityScale = 1;
    }

    protected override void OnUpdate()
    {
        

        base.OnUpdate();

        if (FSM.inPipe) return;

        timeFly -= Time.deltaTime;

        if (timeFly <= 0)
        {
            FSM.rb.velocity = FSM.transform.up * speedMove;
            FSM.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360, 360)));
            timeFly = 1f;
        }
    }
}
