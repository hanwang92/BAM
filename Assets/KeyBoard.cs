using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour {

    Image img;
    Color col;
    int waitTime = 30;
    float fadeSpeed = 0.08f;
    int counter = 0;

    // Use this for initialization
    void Start()
    {
        img = GetComponent<Image>();
        col = img.color;
        col.a = 0;
        img.color = col;
    }

    // Update is called once per frame
    void Update () {
        if (img.enabled)
        {
            if ((counter > waitTime) && (col.a < 0.8f))
            {
                col.a += fadeSpeed;
                img.color = col;
            }
            counter++;
        }
    }
}
