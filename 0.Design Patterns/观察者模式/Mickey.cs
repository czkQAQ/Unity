using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�۲���
public class Mickey : Mouse
{
    public Mickey(string name,Cat cat)
    {
        this.name = name;
        cat.CatAction += Run;
    }

    public override void Run()
    {
        Debug.Log(name + "�����г��������ˡ�");
    }
}
