using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneState : BaseState
{
    public StoneState(Element fsm) : base(fsm) { }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Image/dropTexture");
        FSM.SpriteRenderer.color = new Color(68 / 255f, 24 / 255f, 24 / 255f);
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;

        FSM.Collider.gameObject.tag = "Stone";
        FSM.Trigger.gameObject.tag = "Stone";
    }  
}
