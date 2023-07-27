using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Water")
        {
            collision.gameObject.layer = 4;
        }
        if(collision.tag == "Trap_Lava")
        {
            collision.gameObject.layer = 14;
        }
        if(collision.tag == "Charged")
        {
            collision.gameObject.layer = 10;
        }
        if(collision.tag == "Trap_Gas")
        {
            collision.gameObject.layer = 10;
        }

        collision.gameObject.transform.parent.GetComponent<Element>().inPipe = false;
    }
}
