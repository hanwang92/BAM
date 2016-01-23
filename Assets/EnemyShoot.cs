using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    public Transform laserPrefab;
    public Transform muzzle;
    public Transform target;

    //create and shoot a laser object when firing
    public void Fire(Vector3 dir, Quaternion rot)
    {
        Transform laser = Instantiate(laserPrefab, muzzle.position, muzzle.rotation) as Transform;
        laser.GetComponent<CapsuleCollider>().enabled = true;
        laser.localScale = new Vector3(0.4f, 0.4f, 4.0f);
        laser.GetComponent<Rigidbody>().AddForce(-muzzle.forward * 6000f);
    }
}
