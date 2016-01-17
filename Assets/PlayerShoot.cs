using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Transform cam;
    public Transform aimPivot;
    Vector3 pivot;
    Vector3 target;
    public Transform aimTarget;
    public Transform muzzleFlash;
    public float muzzleVelocity = 1f;
    public Transform bulletPrefab;
    public Transform muzzle;
    public Transform fireTrail;
    
    private LayerMask mask;

    float attackSpeed = 0.001f;
    float cooldown;

    void Start()
    {
        // Add player's own layer to mask
        mask = 1 << gameObject.layer;
        // Add Igbore Raycast layer to mask
        mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
        // Invert mask
        mask = ~mask;

        muzzleFlash.gameObject.SetActive(false);
    }
    
    void Update()
    {
        Aim();
        if ((Time.time >= cooldown) && (!playerHealth.isDamaged) && (!playerHealth.isDead))
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
                StartCoroutine(ShowMuzzleFlash());
            }
        }
        cooldown = Time.time + 0.0165f;
    }

    // Fire a bullet
    void Fire()
    {
        Vector3 dir = aimTarget.position - aimPivot.position;
        dir.Normalize();
        Ray ray = new Ray(aimPivot.position, dir);
        RaycastHit hit;
        var gothit = Physics.Raycast(ray, out hit, 20);
        if (gothit)
        {
            hit.transform.root.SendMessage("OnHit", new RayAndHit(ray, hit), SendMessageOptions.DontRequireReceiver);
            fireTrail.localScale = new Vector3(0.1f, 0.1f, 3.0f);
        }

        /* Draw ray for debugging */
        //Color color = gothit ? Color.green : Color.red;
        //Gizmos.DrawRay(aimPivot.position, dir);
        //Debug.DrawLine(aimPivot.position, aimPivot.position + dir * 1000, color);

        cooldown = Time.time + attackSpeed;
    }

    IEnumerator ShowMuzzleFlash()
    {
        // Show muzzle flash when firing
        muzzleFlash.transform.localRotation *= Quaternion.Euler(0, 0, Random.Range(-360, 360));
        muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.gameObject.SetActive(false);
    }

    void Aim()
    {
        Vector3 dir = aimTarget.position - aimPivot.position;
        Vector3 dir1 = aimTarget.position + aimTarget.up / 6 - aimPivot.position;
        Vector3 dir2 = aimTarget.position - aimTarget.up / 6 - aimPivot.position;
        Vector3 dir3 = aimTarget.position + aimTarget.right / 6 - aimPivot.position;
        Vector3 dir4 = aimTarget.position - aimTarget.right / 6 - aimPivot.position;

        Ray ray = new Ray(aimPivot.position, dir);
        Ray ray1 = new Ray(aimPivot.position, dir1);
        Ray ray2 = new Ray(aimPivot.position, dir2);
        Ray ray3 = new Ray(aimPivot.position, dir3);
        Ray ray4 = new Ray(aimPivot.position, dir4);

        RaycastHit hit;
        var gothit = Physics.Raycast(ray, out hit, 5000) || Physics.Raycast(ray1, out hit, 5000) || Physics.Raycast(ray2, out hit, 5000) || Physics.Raycast(ray3, out hit, 5000) || Physics.Raycast(ray4, out hit, 5000);
        if (gothit)
        {
            hit.transform.root.SendMessage("OnAim", new RayAndHit(ray, hit), SendMessageOptions.DontRequireReceiver);
            hit.transform.root.SendMessage("OnAim", new RayAndHit(ray1, hit), SendMessageOptions.DontRequireReceiver);
            hit.transform.root.SendMessage("OnAim", new RayAndHit(ray2, hit), SendMessageOptions.DontRequireReceiver);
            hit.transform.root.SendMessage("OnAim", new RayAndHit(ray3, hit), SendMessageOptions.DontRequireReceiver);
            hit.transform.root.SendMessage("OnAim", new RayAndHit(ray4, hit), SendMessageOptions.DontRequireReceiver);
        }
        //Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        //Debug.DrawRay(aimPivot.position, dir, Color.green);
        //Debug.DrawRay(aimPivot.position, dir1, Color.red);
        //Debug.DrawRay(aimPivot.position, dir3, Color.blue);
        //Debug.DrawRay(aimPivot.position, dir2, Color.red);
        //Debug.DrawRay(aimPivot.position, dir4, Color.blue);
    }
}
