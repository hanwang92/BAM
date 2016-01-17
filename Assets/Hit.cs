using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(gameObject.tag);
    }

    void OnHit(RayAndHit rayAndHit)
    {
        Debug.Log(gameObject.tag);
    }
}


