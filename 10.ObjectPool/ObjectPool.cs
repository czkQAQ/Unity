using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��򵥵Ķ����ʵ��
public class ObjectPool : MonoBehaviour
{
    //����ģʽ(����д����MonoBehaviour����new��
    /*private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }*/

    //MonoBehaviour�൥������ȷд��
    public static ObjectPool Instance { get;private set; }


    //����Ҫ�洢������
    private GameObject Object => Resources.Load<GameObject>("Prefab/Player");
    //�ڴ��������У�
    private Queue<GameObject> objPool = new Queue<GameObject>();
    //���ӵĳ�ʼ����
    public int defaultCount = 16;
    //���ӵ��������
    public int maxCount = 25;

    private void Awake()
    {
        Instance = this;
    }

    //��ʼ������������
    public void Start()
    {
        GameObject obj;
        for(int i = 0;i < defaultCount;i ++)
        {
            obj = Instantiate(Object, this.transform);
            objPool.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    //�ӳ����л�ȡ
    public GameObject GetFromPool()
    {
        GameObject obj;
        if(objPool.Count > 0)
        {
            obj = objPool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(Object, this.transform);
        }
        return obj;
    }

    //���յ�����
    public void Recycle(GameObject obj)
    {
        if(objPool.Count <= maxCount)
        {
            if (!objPool.Contains(obj))
            {
                objPool.Enqueue(obj);
                obj.SetActive(false);
            }
        }
        else
        {
            Destroy(obj);
        }
    }
}