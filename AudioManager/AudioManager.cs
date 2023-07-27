using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = GameObject.FindObjectOfType<AudioManager>();
                if(instance==null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                    GameObject pool = new GameObject("Pool");
                    instance.PoolTrans = pool.transform;
                    pool.transform.SetParent(obj.transform);
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public Transform PoolTrans { get; private set; }
    AudioPool pool => AudioPool.Instance;
    Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();
    Dictionary<string,AudioSource> cacheDic = new Dictionary<string, AudioSource>();
    bool hasInit;
    public void Init()
    {
        if (hasInit) return;

        hasInit = true;
        LoadConfig();
        pool.Init();
    }

    void LoadConfig()
    {
        var json = Resources.Load<TextAsset>("sound");
        var config=JsonMapper.ToObject<List<AudioItem>>(json.text);
        foreach(var item in config)
        {
            clipDic.Add(item.Name, Resources.Load<AudioClip>(item.Path));
        }
    }

    public void Play(string name, bool loop = false)
    {
        if (!Utils.isSoundOn) return;
        //if (PlayerManager.Instance.winPanel == true)
        //{
        //    return;
        //}

        if (cacheDic.ContainsKey(name)) return;

        var souce = pool.Get();
        souce.clip = clipDic[name];
        souce.loop = loop;
        souce.Play();
        cacheDic.Add(name,souce);
        if(!loop && AudioManager.instance.gameObject.activeSelf)
            StartCoroutine(RemoveCacheWhenOver(clipDic[name].length, name, souce));
    }

    IEnumerator RemoveCacheWhenOver(float second,string name,AudioSource source)
    {
        yield return new WaitForSeconds(second);
        cacheDic.Remove(name);
        pool.Recycle(source);
    }

    public void Stop(string name)
    {
        if (!cacheDic.ContainsKey(name)) return;

        var source = cacheDic[name];
        cacheDic.Remove(name);
        pool.Recycle(source);
    }

    public void StopAll()
    {
        foreach(var name in cacheDic.Keys)
        {
            if (!cacheDic.ContainsKey(name)) return;

            var source = cacheDic[name];
            pool.Recycle(source);
        }
        cacheDic.Clear();
    }
}
