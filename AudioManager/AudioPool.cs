using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    private static AudioPool instance;
    public static AudioPool Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new AudioPool();
            }
            return instance;
        }
    }

    private GameObject audioPrefab = Resources.Load<GameObject>("Prefabs/Audio/AudioSource");
    private int audioCount = 20;

    private Queue<AudioSource> queue=new Queue<AudioSource>();

    public void Init()
    {
        FillPool();
    }

    public void FillPool()
    {
        for(int i = 0;i < audioCount;i++)
        {
            var newAudio = GameObject.Instantiate(audioPrefab);
            newAudio.transform.SetParent(AudioManager.Instance.PoolTrans);
            //newAudio.SetActive(false);
            var source = newAudio.GetComponent<AudioSource>();
            queue.Enqueue(source);
        }
    }


    public AudioSource Get()
    {
        if(queue.Count>0)
        {
            return queue.Dequeue();
        }

        FillPool();
        return queue.Dequeue();
    }


    public void Recycle(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.loop = false;
        queue.Enqueue(source);
    }
}
