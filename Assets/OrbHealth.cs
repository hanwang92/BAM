using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrbHealth : MonoBehaviour
{
    public Transform fireTrail;
    public GameObject GUIPrefab;
    public GameObject head;
    public int startingHealth;                                  // The amount of health the orb starts the game with
    public int currentHealth;                                   // The current health the player has
    public Slider healthSlider;                                 // Reference to the UI's health bar

    public Image background;
    public Image fill;
    public Image name;
    public Image other_background;
    public Image other_fill;                                    
    public Image other_name;                                    

    public bool isDead;                                         // Whether the orb is dead
    bool damaged;                                               // True when the orb gets damaged
    bool aimed;
    int counter = 0;
    int damagedValue;


    void Awake()
    {
        // Set the initial health of the player
        currentHealth = startingHealth;
    }

    void Start()
    {
        currentHealth = startingHealth;
        //Death();
    }


    void Update()
    {
        // Show orb health when aimed by player
        if (aimed)
        {
            background.enabled = true;
            fill.enabled = true;
            name.enabled = true;
            other_background.enabled = false;
            other_fill.enabled = false;
            other_name.enabled = false;
            counter = 0;
        }
        // Hide orb health after certain time not aimed by player
        else if (counter > 200)
        {
            background.enabled = false;
            fill.enabled = false;
            name.enabled = false;
            counter = 0;
        }

        // Reset the damaged flag
        damaged = false;
        aimed = false;
        counter++;
    }


    void OnHit(RayAndHit rayAndHit)
    {
        if (!isDead)
        {
            // Set damage value
            damaged = true;
            damagedValue = Random.Range(1, 5);

            // Reduce the current health by the damage amount
            currentHealth -= damagedValue;

            // Set the health bar's value to the current health
            healthSlider.value = currentHealth;

            //guitext
            guiHealth();
        }
        

        // If the orb has lost all it's health it should die
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void OnAim(RayAndHit rayAndHit)
    {
        aimed = true;
    }

    void Death()
    {
        isDead = true;
        StartCoroutine(Die());
    }

    // Damage health points popping up from enemy
    void guiHealth()
    {
        float rand_x = Random.Range(-0.2f, 0.2f);
        float rand_y = Random.Range(0.8f, 1.0f);
        float rand_z = Random.Range(-0.3f, 0.3f);
        Vector3 spawn = new Vector3(head.transform.position.x + rand_x, head.transform.position.y + rand_y, head.transform.position.z + rand_z);
        GameObject GUIDamage = Instantiate(GUIPrefab, Camera.main.WorldToViewportPoint(spawn), Quaternion.identity) as GameObject;
        GUIText text = GUIDamage.GetComponent<GUIText>();
        text.text = damagedValue.ToString();
        Destroy(GUIDamage, 0.5f);
    }

    IEnumerator Die()
    {
        GetComponent<Animation>().Stop();
        fireTrail.gameObject.SetActive(false);

        OrbController oc = GetComponent<OrbController>();

        // Disable this component so Update and LateUpdate is no longer called
        oc.enabled = false;

        // Fold turret out from whatever point it was in the animation (unless it's already fully out)
        if (oc.IsUnfolded() == false || GetComponent<Animation>()["FoldTurret"].enabled)
        {
            GetComponent<Animation>()["FoldTurret"].enabled = true;
            GetComponent<Animation>()["FoldTurret"].speed = 1;
        }

        // Give the orb a rigidbody so it can move physically
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        // Make the orb rotate wildly in air for 1.5 seconds
        float fallAfterSeconds = 1.5f;
        float stopAfterSeconds = 2.7f;
        float rotateSpeed = 2000f;
        float axisChange = 20f;

        float time = Time.time;
        Vector3 axis = Vector3.up;
        while (Time.time < time + stopAfterSeconds)
        {
            if (Time.deltaTime > 0 && Time.timeScale > 0)
            {

                // Value that starts at 0 and is 1 after fallAfterSeconds time
                float fallLerp = Mathf.InverseLerp(time, time + fallAfterSeconds, Time.time);

                // Value that starts at 0 and is 1 after stopAfterSeconds time
                float explodeLerp = Mathf.InverseLerp(time, time + stopAfterSeconds, Time.time);

                // Rotate the axis to create unpredictable rotation
                float deltaRot = axisChange * Time.deltaTime;
                axis = Quaternion.Euler(deltaRot, deltaRot, deltaRot) * axis;

                // Rotate around the axis.
                GetComponent<Rigidbody>().angularVelocity = axis * fallLerp * rotateSpeed * Mathf.Deg2Rad;
                
                // Make it fall to the ground after fallAfterSeconds time
                if (Time.time - time > fallAfterSeconds && rigid.useGravity == false)
                {
                    rigid.useGravity = true;
                    axisChange = 90f;
                }
            }
            yield return 0;
        }

        // Interpolate orb velocity from dropping to complete stop
        GetComponent<Rigidbody>().angularVelocity = Vector3.Lerp(GetComponent<Rigidbody>().angularVelocity, Vector3.zero, 10000);
        GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, 10000);
    }
}