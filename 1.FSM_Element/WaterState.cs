using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterState : BaseState
{
    public WaterState(Element fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Image/dropTexture");
        FSM.SpriteRenderer.color = new Color(0 / 255f, 191 / 255f, 255 / 255f, 130f / 255);
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;

        FSM.Collider.gameObject.tag = "Water";
        FSM.Trigger.gameObject.tag = "Water";
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Lava")
        {
            FSM.Transition(STATESTYPE.STONE);
        }
    }
}
