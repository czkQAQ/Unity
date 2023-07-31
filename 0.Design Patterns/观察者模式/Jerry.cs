using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//观察者
public class Jerry : Mouse
{
    public Jerry(string name,Cat cat)
    {
        this.name = name;
        cat.CatAction += Run;
    }

    public override void Run()
    {
        Debug.Log(name + "跑步，逃跑了。");
    }
}
