using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ����ģʽ
    ����ģʽ�漰��һ����һ���࣬���ฺ�𴴽��Լ��Ķ���ͬʱȷ��ֻ�е������󱻴�����������ṩ��һ�ַ�����Ψһ�Ķ���ķ�ʽ������ֱ�ӷ��ʣ�����Ҫʵ��������Ķ���
 */


/*
    ����һ������ʽ
    ����һ�η���Instanceʱ��Ϊinstanceʵ���������Ǻ����������˷���ʱ�ų�ʼ���������Խ�����ʽ��
    ��֧�ֶ��̡߳�
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
    ������������ʽ
    ���౻����ʱ�ͳ�ʼ�������Խж���ʽ��
    ֧�ֶ��̣߳��������ʱ�ͳ�ʼ�����˷��ڴ档
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