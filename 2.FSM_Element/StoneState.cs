using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StoneState : BaseState
{
    public StoneState(Element fsm) : base(fsm) { }
    protected override void OnInit()
    {
        if (FSM.InitialState == STATETYPE.STONE)
        {
            AudioManager.Instance.Play(AudioConstant.rock1);
        }
    }

    protected override void OnEnter()
    {
        /*FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/Lava_Stone");
        FSM.SpriteRenderer.color = Color.white;
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;
        FSM.Collider.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics Materials/Regular Water.physicsMaterial2D");
        FSM.Trigger.tag = "Tag_Stone";*/
        FSM.StartCoroutine(DelayToChange());
        

        //FSM.gameObject.transform.position = new Vector3(FSM.gameObject.transform.position.x, FSM.gameObject.transform.position.y, -1);
    }

    protected override void OnTransition()
    {
        //ElementAudioNew.Instance.PlayAudio(ElementAudioNew.Type.STONE);
        AudioManager.Instance.Play(AudioConstant.lavaonwater2);
    }

    

    IEnumerator DelayToChange()
    {
        yield return new WaitForSeconds(0.1f);

        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/Stone_small");
        FSM.SpriteRenderer.color = Color.white;
        FSM.SpriteRenderer.transform.localScale = Vector3.one * 0.3f;
        FSM.Collider.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Physics Materials/Regular Water.physicsMaterial2D");
        FSM.Collider.tag = "Tag_Stone";
        FSM.Trigger.tag = "Tag_Stone";
        FSM.Collider.gameObject.layer = 9;
        FSM.SetDelayTrigger();

        FSM.GetComponent<Rigidbody2D>().mass = 1;
    }
}
