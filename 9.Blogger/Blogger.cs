using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    委托
    委托是一种容器，容器里面放的是函数方法。而函数的形式各不相同，参数，返回值各不相同，所以你做委托之前，先得要定义好这个委托容器存放的函数的类型,即委托类型。
    定义了好了函数类型后，将函数加入到委托容器后，你只要触发委托调用，委托就会帮你把容器里面的每个函数都调用一次,触发的时候和调用普通函数没有区别。
 */

/*
    举一个例子，你关注了一个博主，当这个博主发布了这个专栏的一个最新文章，就会收到消息。这个就是用的委托，实现如下
 */

public delegate void Subscribe(string name);//关注委托，用于存放订阅人员的推送方法

public class Blogger : MonoBehaviour
{
    public static Subscribe subscribe;

    public InputField articleName;
    public Button uploadBtn;

    private void Start()
    {
        //按钮监听
        uploadBtn.onClick.AddListener(Upload);
    }

    private void Upload()
    {
        if(articleName.text != string.Empty)
        {
            Debug.Log("博主上传了文章" + articleName.text);
            subscribe(articleName.text);
        }
    }
}
