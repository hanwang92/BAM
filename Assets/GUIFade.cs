using UnityEngine;
using System.Collections;

public class GUIFade : MonoBehaviour {

    public Gameflow gameFlow;
    private float time_;
    float time_to_fade;
    int counter;

	// Use this for initialization
	void Start () {
        time_ = Time.time;
        time_to_fade = 0.5f;
        counter = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if ((counter > 10) && (!gameFlow.inPause))
        {
            //Damage health slowly moves upward
            transform.Translate(0, 0.001f, 0);
            Color textColor = transform.GetComponent<GUIText>().color;

            //Damage health slowly fades away
            textColor.a = Mathf.Cos((Time.time - time_) * ((Mathf.PI / 2) / time_to_fade));
            if (gameFlow.inGameOver)
            {
                textColor.a = 0;
            }
            transform.GetComponent<GUIText>().color = textColor;
        }
        counter++;
        
        
    }
}
