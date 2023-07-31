using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    观察者模式
    当对象间存在一对多关系时，则使用观察者模式（Observer Pattern）。比如，当一个对象被修改时，则会自动通知依赖它的对象。观察者模式属于行为型模式。
    举例：我们用猫抓老鼠的来举例子。当汤姆猫进屋子的时候，杰瑞用跑步的方式逃跑，米老鼠骑自行车逃跑，舒克开坦克逃跑。这个时候汤姆猫就是被观察者，有
一个进屋子的方法。杰瑞，米老鼠，舒克就是观察者都有一个逃跑的方法。
    优点： 1、观察者和被观察者是抽象耦合的，使用委托在观察者中注册事件更能大大降低耦合。 2、建立一套触发机制。
 */


//被观察者，猫类
public class Cat : MonoBehaviour
{
    private string _name;
    public string Name { get; private set; }
    private string _color;
    public string Color { get; private set; }

    public Cat(string name,string color)
    {
        this.Name = name;
        this.Color = color;
    }

    public event Action CatAction;

    public void CatIntoRoom()
    {
        Debug.Log("叫" + Name + "的" + Color + "猫，进屋了。");
        CatAction?.Invoke();
    }
}
