using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//观察者
public class Mickey : Mouse
{
    public Mickey(string name,Cat cat)
    {
        this.name = name;
        cat.CatAction += Run;
    }

    public override void Run()
    {
        Debug.Log(name + "骑自行车，逃跑了。");
    }
}
