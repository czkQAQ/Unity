using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInit : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.Instance.Init();
    }
}
