using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    private void Start()
    {
        Cat cat = new Cat("��ķ", "��ɫ");
        Jerry jerry = new Jerry("����", cat);
        Mickey mickey = new Mickey("����", cat);
        Schuck schucl = new Schuck("���", cat);

        cat.CatIntoRoom();
    }
}
