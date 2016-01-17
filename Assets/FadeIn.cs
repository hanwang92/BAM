using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    
    public void FadeInFunc(int counter, Image img, int waitTime, float fadeSpeed, Color col)
    {
        if (counter > waitTime)
        {
            col.a += fadeSpeed;
            img.color = col;
        }
        counter++;        
    }
}
