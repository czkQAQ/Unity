using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    单例模式
    这种模式涉及到一个单一的类，该类负责创建自己的对象，同时确保只有单个对象被创建。这个类提供了一种访问其唯一的对象的方式，可以直接访问，不需要实例化该类的对象。
 */


/*
    方法一：懒汉式
    当第一次访问Instance时，为instance实例化，就是很懒（其他人访问时才初始化），所以叫懒汉式。
    不支持多线程。
 */
public class SingletonPattern
{
    private static SingletonPattern instance;
    public static SingletonPattern Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SingletonPattern();
            }
            return instance;
        }
    }

    public void Func()
    {
        //DoSomething
    }
}


/*
    方法二：饿汉式
    当类被加载时就初始化，所以叫饿汉式。
    支持多线程，但类加载时就初始化，浪费内存。
 */
public class SingletonPattern2
{
    private static SingletonPattern2 instance = new SingletonPattern2();
    public static SingletonPattern2 Instance
    {
        get
        {
            return instance;
        }
    }

    public void Func()
    {
        //DoSomething
    }
}