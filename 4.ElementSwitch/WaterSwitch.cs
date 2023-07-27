﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSwitch : BaseSwitch
{
    public WaterSwitch(ElementSwitch fsm) : base(fsm) { }

    protected override void OnEnter()
    {
        FSM.SpriteRenderer.sprite = Resources.Load<Sprite>("Images/InGame/Game_objects/Trap/trun_machine_water");
        FSM.SpriteRenderer.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + FSM.rotate));
    }

    //public static bool inSwitch;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Trap_Gas")
        {
            //inSwitch = true;
            collision.gameObject.transform.parent.GetComponent<Element>().inSwitch = true;
            collision.gameObject.transform.parent.GetComponent<Element>().inWaterSwitch = true;

            collision.gameObject.transform.parent.GetComponent<Element>()?.Transition(STATETYPE.WATER);

        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Trap_Gas")
        {
            //inSwitch = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Trap_Lava" || collision.tag == "Charged" || collision.tag == "Trap_Gas" || collision.tag == "Water")
        {
            collision.gameObject.transform.parent.GetComponent<Element>().inSwitch = false;
            collision.gameObject.transform.parent.GetComponent<Element>().inWaterSwitch = false;

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
