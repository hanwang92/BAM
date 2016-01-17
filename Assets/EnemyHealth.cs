using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject GUIPrefab;
    public GameObject head;
    public int startingHealth = 500;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.

    public Image background;
    public Image fill;
    public Image name;
    public Image other_background;
    public Image other_fill;
    public Image other_name;
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.
    bool aimed;
    int counter = 0;
    int damagedValue;
    
    void Awake()
    {
        // Set the initial health of the soldier.
        currentHealth = startingHealth;
    }

    void Start()
    {
        currentHealth = startingHealth;
        background.enabled = false;
        fill.enabled = false;
        name.enabled = false;
        other_background.enabled = false;
        other_fill.enabled = false;
        other_name.enabled = false;
    }

    void Update()
    {
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
        else if (counter > 200)
        {
            background.enabled = false;
            fill.enabled = false;
            name.enabled = false;
            counter = 0;
        }

        // Reset the damaged flag.
        damaged = false;
        aimed = false;
        counter++;
    }

    void OnHit(RayAndHit rayAndHit)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;
        damagedValue = Random.Range(1, 5);

        // Reduce the current health by the damage amount.
        currentHealth -= damagedValue;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        //guitext
        guiHealth();

        // If the soldier has lost all it's health
        if (currentHealth <= 0 && !isDead)
        {
            // Game over.
            Death();
        }
    }

    void OnAim(RayAndHit rayAndHit)
    {
        aimed = true;
    }

    void Death()
    {
        //Win
    }
    
    //damage health points popping up from enemy
    void guiHealth ()
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
}