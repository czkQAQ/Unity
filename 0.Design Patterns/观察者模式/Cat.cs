using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    �۲���ģʽ
    ����������һ�Զ��ϵʱ����ʹ�ù۲���ģʽ��Observer Pattern�������磬��һ�������޸�ʱ������Զ�֪ͨ�������Ķ��󡣹۲���ģʽ������Ϊ��ģʽ��
    ������������èץ������������ӡ�����ķè�����ӵ�ʱ�򣬽������ܲ��ķ�ʽ���ܣ������������г����ܣ���˿�̹�����ܡ����ʱ����ķè���Ǳ��۲��ߣ���
һ�������ӵķ�����������������˾��ǹ۲��߶���һ�����ܵķ�����
    �ŵ㣺 1���۲��ߺͱ��۲����ǳ�����ϵģ�ʹ��ί���ڹ۲�����ע���¼����ܴ�󽵵���ϡ� 2������һ�״������ơ�
 */


//���۲��ߣ�è��
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
        Debug.Log("��" + Name + "��" + Color + "è�������ˡ�");
        CatAction?.Invoke();
    }
}
