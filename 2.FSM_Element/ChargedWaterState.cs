using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWaterState : BaseState
{
    public ChargedWaterState(Element fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.color = new Color(0 / 255f, 191 / 255f, 255 / 255f, 200 / 255f);
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/dropTexture");
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.035f;
        FSM.Collider.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics Materials/Bouncy Water");

        FSM.Trigger.gameObject.tag = "Charged";

        FSM.Collider.gameObject.tag = "Charged";
        FSM.Collider.gameObject.layer = 0;//与enenmy碰撞即可

        var effect = Resources.Load<GameObject>("EFFECT Tuyet/Prefab/Rescue Hero/Electricity");
        GameObject.Instantiate(effect, FSM.EffectPlace);

        FSM.SetDelayTrigger();

        FSM.radius = 0.07f;
    }

    protected override void OnExit()
    {
        FSM.SetDelayTrigger();
    }

    protected override void OnTransition()
    {
        //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Type.CHARGEDWATER);
        AudioManager.Instance.Play(AudioConstant.electric);
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "BodyPlayer")
    //    {
    //        if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
    //        {
    //            if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
    //            {
    //                PlayerManager.Instance.OnPlayerDie(true);
    //            }
    //        }
    //    }
    //    if (collision.tag == "Trap_Lava")
    //    {
    //        FSM.Transition(STATETYPE.STONE);
    //    }
    //    if (collision.tag == "Tag_Stone" && !collision.gameObject.name.Contains("FallingStone"))
    //    {
    //        FSM.Transition(STATETYPE.STONE);
    //    }
    //    else if (collision.tag == "fire")
    //    {
    //        FSM.StartCoroutine(DelayToChange2Steam());
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
            if (hit.tag == "BodyPlayer")
            {
                if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
                {
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        PlayerManager.Instance.OnPlayerDie(true);
                    }
                }
            }
            if (hit.tag == "Trap_Lava")
            {
                //if (PoisonSwitch.inSwitch == true) return;
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            if (hit.tag == "Tag_Stone" && !hit.gameObject.name.Contains("FallingStone"))
            {
                //if (PoisonSwitch.inSwitch == true) return;
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            else if (hit.tag == "fire")
            {
                FSM.StartCoroutine(DelayToChange2Steam());
            }
        }
    }

    IEnumerator DelayToChange2Steam()
    {
        float t = Random.Range(1f, 2f);
        Debug.Log(t);
        yield return new WaitForSeconds(t);

        FSM.Transition(STATETYPE.STEAM);
        Debug.Log("change2Steam");
    }
}
