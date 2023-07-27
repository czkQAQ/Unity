using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Water" || collision.tag == "Trap_Lava" || collision.tag == "Trap_Gas" || collision.tag == "Charged")
        {
            collision.gameObject.layer = 31;
            collision.gameObject.transform.parent.GetComponent<Element>().inPipe = true;
        }
        
    }
}
