using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSwitch : BaseSwitch {
    public PoisonSwitch(ElementSwitch fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/Game_objects/Trap/trun_machine_poison");
        FSM.SpriteRenderer.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + FSM.rotate));
    }

    //public static bool inSwitch;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Water")
        {
            //inSwitch = true;
            collision.gameObject.transform.parent.GetComponent<Element>().inSwitch = true;
            collision.gameObject.transform.parent.GetComponent<Element>().inPoisonSwitch = true;

            collision.gameObject.transform.parent.GetComponent<Element>()?.Transition(STATETYPE.Poison);

        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Water")
        {
            //inSwitch = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Water")
        {
            collision.gameObject.transform.parent.GetComponent<Element>().inSwitch = false;
            collision.gameObject.transform.parent.GetComponent<Element>().inPoisonSwitch = false;


        }
    }
    //protected override void OnUpdate()
    //{
    //    base.OnUpdate();
    //    if (FSM.noStay)
    //    {
    //        inSwitch = false;
    //    }
    //    else
    //    {
    //        inSwitch = true;
    //    }
    //}
}
