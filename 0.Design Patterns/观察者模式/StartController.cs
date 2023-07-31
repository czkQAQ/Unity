using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    private void Start()
    {
        Cat cat = new Cat("ÌÀÄ·", "À¶É«");
        Jerry jerry = new Jerry("½ÜÈð", cat);
        Mickey mickey = new Mickey("Ã×Ææ", cat);
        Schuck schucl = new Schuck("Êæ¿Ë", cat);

        cat.CatIntoRoom();
    }
}
