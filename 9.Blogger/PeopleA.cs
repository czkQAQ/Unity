using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleA : MonoBehaviour
{
    void Start()
    {
        //����ί������ӷ���
        Blogger.subscribe += Push;
    }

    private void Push(string name)
    {
        Debug.Log("����PeopleA���ҽ��յ���" + name);
    }
}
