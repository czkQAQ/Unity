using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    ί��
    ί����һ����������������ŵ��Ǻ�������������������ʽ������ͬ������������ֵ������ͬ����������ί��֮ǰ���ȵ�Ҫ��������ί��������ŵĺ���������,��ί�����͡�
    �����˺��˺������ͺ󣬽��������뵽ί����������ֻҪ����ί�е��ã�ί�оͻ��������������ÿ������������һ��,������ʱ��͵�����ͨ����û������
 */

/*
    ��һ�����ӣ����ע��һ��������������������������ר����һ���������£��ͻ��յ���Ϣ����������õ�ί�У�ʵ������
 */

public delegate void Subscribe(string name);//��עί�У����ڴ�Ŷ�����Ա�����ͷ���

public class Blogger : MonoBehaviour
{
    public static Subscribe subscribe;

    public InputField articleName;
    public Button uploadBtn;

    private void Start()
    {
        //��ť����
        uploadBtn.onClick.AddListener(Upload);
    }

    private void Upload()
    {
        if(articleName.text != string.Empty)
        {
            Debug.Log("�����ϴ�������" + articleName.text);
            subscribe(articleName.text);
        }
    }
}
