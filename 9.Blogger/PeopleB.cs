using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleB : MonoBehaviour
{
    void Start()
    {
        //����ί������ӷ���
        Blogger.subscribe += Push;
    }

    private void Push(string name)
    {
        Debug.Log("����PeopleB���ҽ��յ���" + name);
    }
}
