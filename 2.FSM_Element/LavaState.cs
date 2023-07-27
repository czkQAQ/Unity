using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LavaState : BaseState
{
    public LavaState(Element fsm) : base(fsm) { }

    public bool isChanged;

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.color = new Color(255 / 255f, 91 / 255f, 0 / 255f);

        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/dropTexture");
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.035f;

        FSM.Trigger.gameObject.tag = "Trap_Lava";
        FSM.Collider.gameObject.tag = "Trap_Lava";
        FSM.Collider.gameObject.layer = 14;
        var effect = Resources.Load<GameObject>("EFFECT Tuyet/Prefab/Rescue Hero/fire 1");
        effect.transform.localScale = Vector3.one * 0.7f;
        effect.transform.localPosition = Vector3.zero + new Vector3(0, 0, 1);
        GameObject.Instantiate(effect, FSM.EffectPlace);
        var effect2 = Resources.Load<GameObject>("EFFECT Tuyet/Prefab/Rescue Hero/fire (Ingame)");
        effect2.transform.localScale = Vector3.one * 0.5f;
        effect2.transform.localPosition = Vector3.zero;
        GameObject.Instantiate(effect2, FSM.EffectPlace);
        FSM.SpriteRenderer.gameObject.layer = 14;

        if (!isChanged)
        {
            //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Instance.otherAudio, ElementAudioNew.Instance.acLavaAppear);
            AudioManager.Instance.Play(AudioConstant.lavaappear);
            isChanged = true;
        }

        FSM.radius = 0.07f;
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Tag_Stone" && collision.gameObject.name != "FallingStone")
    //    {
    //        FSM.Transition(STATETYPE.STONE);
    //    }
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
    //    if (collision.gameObject.tag == "Ice" || collision.tag == "BrokenIce")
    //    {
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
            if (hit.tag == "Tag_Stone" && hit.gameObject.name != "FallingStone")
            {
                FSM.Transition(STATETYPE.STONE);
            }
            if (hit.tag == "BodyPlayer")
            {
                if (FSM.Collider.gameObject.layer == 31) return;
                if (PlayerManager.Instance.pState == PlayerManager.P_STATE.PLAYING || PlayerManager.Instance.pState == PlayerManager.P_STATE.RUNNING)
                {
                    if (GameManager.Instance.gameState != GameManager.GAMESTATE.WIN)
                    {
                        PlayerManager.Instance.OnPlayerDie(true);
                    }
                }
            }
            if (hit.tag == "Ice" || hit.tag == "BrokenIce")
            {
                FSM.Transition(STATETYPE.STONE);
            }
        }

    }
}
