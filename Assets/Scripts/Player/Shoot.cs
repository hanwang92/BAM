using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
    
    //public Rigidbody bulletPrefab;
    public Transform cam;

    public Transform aimPivot;
	public Transform aimWeapon;
	public Transform aimTarget;
	public float effect = 1;
	
	private Vector3 aimDirection = Vector3.zero;
    private LayerMask mask;

    //EnemyHealth enemyHealth;      
    float attackSpeed = 0.001f;
    float cooldown;
    public float speed;
    public int damage;

    void Awake()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //enemyHealth = enemy.GetComponent<EnemyHealth>();
    }

    void Update()
    {
        //Quaternion camRot = new Quaternion(cam.rotation.x, cam.rotation.y, cam.rotation.z, cam.rotation.w);
        //transform.rotation = camRot;

        if (Time.time >= cooldown)
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
            }
        }

    }

    // Fire a bullet
    void Fire()
    {
        Debug.Log("fire");
        //Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
        //Rigidbody bPrefab = Instantiate(bulletPrefab, pos, cam.rotation) as Rigidbody;
        //bPrefab.transform.localScale = new Vector3(0.5f,0.5f,0.5f);

        //bPrefab.transform.Rotate(-90,0,-15);
        //bPrefab.AddForce(Vector3.forward * 100f);
        //bPrefab.velocity = transform.TransformDirection(new Vector3(0, 0, speed));

        
        Debug.Log(aimTarget.position);
        Debug.Log(aimPivot.position);

        //aimTarget.position += cam.up;

        //Vector3 dir = aimTarget.position - aimPivot.position;
        Vector3 dir = aimTarget.position - cam.up*0.5f - cam.right*0.2f - aimPivot.position;
        dir.Normalize();
        Ray ray = new Ray(aimPivot.position, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000))
        {
            Debug.Log("here");
            hit.transform.root.SendMessage("OnHit", new RayAndHit(ray, hit), SendMessageOptions.DontRequireReceiver);
        }

        cooldown = Time.time + attackSpeed;
        
    }
}
