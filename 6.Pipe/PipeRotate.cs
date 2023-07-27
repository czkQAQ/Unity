using EG.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRotate : MonoBehaviour
{
    public GameObject pipeUp;
    public GameObject pipeLeft;
    public GameObject pipeDown;
    public GameObject pipeRight;
    public bool over;

    public GameObject Light;

    private void Update()
    {
        if(transform.up == new Vector3(0, 1))
        {
            pipeUp.SetActive(true);
            pipeLeft.SetActive(false);
            pipeRight.SetActive(false);
            pipeDown.SetActive(false);
        }else if(transform.up == new Vector3(-1, 0))
        {
            pipeUp.SetActive(false);
            pipeLeft.SetActive(true);
            pipeRight.SetActive(false);
            pipeDown.SetActive(false);
        }else if(transform.up == new Vector3(0, -1))
        {
            pipeUp.SetActive(false);
            pipeLeft.SetActive(false);
            pipeRight.SetActive(false);
            pipeDown.SetActive(true);
        }else if(transform.up == new Vector3(1, 0))
        {
            pipeUp.SetActive(false);
            pipeLeft.SetActive(false);
            pipeRight.SetActive(true);
            pipeDown.SetActive(false);
        }        
    }

    private void OnMouseEnter()
    {
        over = true;
    }

    private void OnMouseExit()
    {
        over = false;
    }

    private void OnMouseDown()
    {
        if (over)
        {
            if (Light.activeSelf == true) Light.SetActive(false);
            transform.eulerAngles += new Vector3(0, 0, 90);
            EventManager.Instance.Trigger<EventTipsDissolve>();

        }
    }
}
