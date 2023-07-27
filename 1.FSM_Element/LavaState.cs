using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaState : BaseState
{
    public LavaState(Element fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Image/dropTexture");
        FSM.SpriteRenderer.color = new Color(255 / 255f, 91 / 255f, 0 / 255f);
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;

        FSM.Collider.gameObject.tag = "Lava";
        FSM.Trigger.gameObject.tag = "Lava";
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Water")
        {
            FSM.Transition(STATESTYPE.STONE);
        }
    }
}
