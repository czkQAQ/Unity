using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleB : MonoBehaviour
{
    void Start()
    {
        //向订阅委托中添加方法
        Blogger.subscribe += Push;
    }

    private void Push(string name)
    {
        Debug.Log("我是PeopleB，我接收到了" + name);
    }
}
