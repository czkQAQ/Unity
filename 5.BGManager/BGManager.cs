using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;

public class BGManager : MonoBehaviour
{
    private static BGManager instance;
    public static BGManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<BGManager>();
                if(instance == null)
                {
                    GameObject obj = new GameObject("BGManager");
                    instance = obj.AddComponent<BGManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    Dictionary<string, string> BGDic = new Dictionary<string, string>();
    Dictionary<string, Sprite> SpriteDic = new Dictionary<string, Sprite>();
    private GameObject Level;
    private GameObject Background;


    public void Init()
    {
        
        LoadConfig();
        AddSprite();
    }

    void LoadConfig()
    {
        var json = Resources.Load<TextAsset>("level");
        var config = JsonMapper.ToObject<List<BGItem>>(json.text);
        foreach (var item in config)
        {
            BGDic.Add(item.KeyName + "(Clone)",item.background);
        }
    }

    void AddSprite()
    {
        //SpriteDic.Add("bg0", Resources.Load<Sprite>("Images/InGame/Game_Background/bg0"));
        //SpriteDic.Add("bg1", Resources.Load<Sprite>("Images/InGame/Game_Background/bg1"));
        //SpriteDic.Add("bg2", Resources.Load<Sprite>("Images/InGame/Game_Background/bg2"));
        //SpriteDic.Add("bg3", Resources.Load<Sprite>("Images/InGame/Game_Background/bg3"));

        for (int i = 0;i < 100;i ++)
        {
            SpriteDic.Add("bg" + i.ToString(), Resources.Load<Sprite>("Images/InGame/Game_Background/bg" + i));
        }
    }


    /*private void Update()
    {
        if (Application.loadedLevelName.Equals("MainGame"))
        {
            Level = GameObject.FindObjectOfType<MapLevelManager>().gameObject;
            

            string bgType = BGDic[Level.name];
            Background.gameObject.GetComponent<Image>().sprite = SpriteDic[bgType];
        }
    }*/

    public void SetBg(string name)
    {
        string bgType = BGDic[name];
        Background = GameObject.Find("Panel-Background");
        Background.gameObject.GetComponent<Image>().sprite = SpriteDic[bgType];
    }
}
