using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFromPool : MonoBehaviour
{
    public Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(Creat);
    }

    private void Creat()
    {
        GameObject obj = ObjectPool.Instance.GetFromPool();
        StartCoroutine(DelayRecycle(obj));
    }

    IEnumerator DelayRecycle(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        ObjectPool.Instance.Recycle(obj);
    }
}
