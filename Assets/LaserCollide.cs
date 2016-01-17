using UnityEngine;
using System.Collections;

public class LaserCollide : MonoBehaviour {

    public PlayerHealth playerHealth;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Bullet destroys itself
            Destroy(gameObject);
            playerHealth.TakeDamage(30);
        }
        if (other.gameObject.tag == "Floor")
        {
            //Bullet destroys itself
            Destroy(gameObject);
        }
    }
}
