using UnityEngine;
using System.Collections;

public class Tilt : MonoBehaviour {

    public Transform cam;

    // Update is called once per frame
    void LateUpdate () {
        Quaternion camRot = new Quaternion(cam.rotation.x, cam.rotation.y, cam.rotation.z, cam.rotation.w);
        transform.rotation = camRot;
    }
}
