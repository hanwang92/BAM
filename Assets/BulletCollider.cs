using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

    //public Shoot shoot;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
        // bullet looks for the EnemyHealth Script
        //other.GetComponent<EnemyHealth>();
        // bullet calls the function in the EnemyHealth Script and takes away the amount you will set in the inspector as Damage
        //enemyHealth.TakeDamage(damage);
        // bullet Destorys itself
        //Destroy(shoot.bPrefab.gameObject);
            Debug.Log("here");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("here1");
            Destroy(gameObject);
        }
        
    }
}
