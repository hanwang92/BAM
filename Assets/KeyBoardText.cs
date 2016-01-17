using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyBoardText : MonoBehaviour {

    Image img;
    Color col;
    float waitTime = 30f;
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
    void Update()
    {
        if (img.enabled)
        {
            if (counter >= waitTime * Mathf.PI)
            {
                col.a = Mathf.Sin(counter/30f);
                img.color = col;
            }
            counter++;
        }
    }
}
