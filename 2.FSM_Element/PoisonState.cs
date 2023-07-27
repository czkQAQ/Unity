using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonState : BaseState
{
    public PoisonState(Element fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/dropTexture");
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.035f;

        FSM.SpriteRenderer.color = new Color(35 / 255f, 224 / 255f, 39 / 255f, 130f / 255);
        var effect = Resources.Load<GameObject>("EFFECT Tuyet/Prefab/Rescue Hero/water");
        GameObject.Instantiate(effect, FSM.EffectPlace);
        effect.transform.localPosition = Vector3.zero;

        //FSM.Trigger.tag = "Water";
        FSM.SpriteRenderer.gameObject.layer = 24;
        FSM.Collider.gameObject.tag = "Trap_Gas";

        FSM.radius = 0.07f;
    }

    protected override void OnTransition()
    {
        AudioManager.Instance.Play(AudioConstant.water_drop);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }



    protected override void OnHitEnter(Collider2D[] hits)
    {
        foreach (var hit in hits)
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
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                //if (PoisonSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            else if (hit.tag == "Charged")
            {
                //if (PoisonSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.CHARGEDWATER);
            }
            else if (hit.tag == "Tag_Stone" && hit.gameObject.name != "FallingStone")
            {
                //if (ChargedWaterSwitch.inSwitch == true) return;
                //if (LavaSwitch.inSwitch == true) return;
                //if (WaterSwitch.inSwitch == true) return;
                //if (PoisonSwitch.inSwitch == true) return;
                FSM.Transition(STATETYPE.STONE);
            }
            else if (hit.tag == "fire")
            {
                FSM.StartCoroutine(DelayToChangeToGas());
            }
            else if (hit.tag == "Ice" || hit.tag == "BrokenIce")
            {                        
                    FSM.Transition(STATETYPE.BrokenIce);               
            }
        }
    }

    IEnumerator DelayToChangeToGas()
    {
        float t = Random.Range(1f, 2f);
        yield return new WaitForSeconds(t);

        if (FSM.Collider.gameObject.layer == 24)
        {
            FSM.Transition(STATETYPE.Gas);
        }
    }
}
