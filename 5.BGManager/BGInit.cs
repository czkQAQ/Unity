using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGInit : MonoBehaviour
{
    private void Awake()
    {
        BGManager.Instance.Init();
    }
}
