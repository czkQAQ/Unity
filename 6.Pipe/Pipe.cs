using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private Vector2 targetPos;
    private Vector2 vDir;
    public float speed;

    private void Start()
    {
        vDir = -transform.up;
    }

    public static bool inPipe;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water" || collision.tag == "Trap_Lava" || collision.tag == "Trap_Gas" || collision.tag == "Charged")
        {
            inPipe = true;
            //collision.gameObject.transform.parent.GetComponent<Rigidbody2D>().simulated = false;

            //collision.gameObject.transform.parent.transform.localPosition =
            //    Vector2.Lerp(collision.gameObject.transform.parent.transform.localPosition, targetPos, Time.deltaTime * speed);
            collision.transform.parent.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.transform.parent.GetComponent<Rigidbody2D>().velocity = vDir * speed;
            collision.transform.parent.GetComponent<Rigidbody2D>().drag = 100;
            collision.transform.parent.GetComponent<Element>().radius = 0;
       
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water" || collision.tag == "Trap_Lava" || collision.tag == "Trap_Gas" || collision.tag == "Charged")
        {
            inPipe = false;
            collision.transform.parent.GetComponent<Rigidbody2D>().gravityScale = 1;
            //collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.transform.parent.GetComponent<Rigidbody2D>().drag = 0;
            collision.transform.parent.GetComponent<Element>().radius = 0.07f;
        }   
    }
}
