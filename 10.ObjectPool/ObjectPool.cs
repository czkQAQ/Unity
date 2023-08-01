using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//最简单的对象池实现
public class ObjectPool : MonoBehaviour
{
    //单例模式(错误写法，MonoBehaviour不能new）
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

    //MonoBehaviour类单例的正确写法
    public static ObjectPool Instance { get;private set; }


    //池子要存储的物体
    private GameObject Object => Resources.Load<GameObject>("Prefab/Player");
    //内存区（队列）
    private Queue<GameObject> objPool = new Queue<GameObject>();
    //池子的初始容量
    public int defaultCount = 16;
    //池子的最大容量
    public int maxCount = 25;

    private void Awake()
    {
        Instance = this;
    }

    //初始化，填满池子
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

    //从池子中获取
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

    //回收到池子
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