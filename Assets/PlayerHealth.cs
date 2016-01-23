using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    public int startingHealth = 100;                            // The amount of health the player starts the game with
    public int currentHealth;                                   // The current health the player has
    public Slider healthSlider;                                 // Reference to the UI's health bar

    public bool isDead;                                         // Whether the player is dead
    public bool isDamaged;                                      // True when the player gets damaged

    
    void Awake()
    {
        // Set the initial health of the player
        currentHealth = startingHealth;
    }
    
    public void TakeDamage(int amount)
    {
        isDamaged = true;

        // Reduce the current health by the damage amount
        currentHealth -= amount;

        // Set the health bar's value to the current health
        healthSlider.value = currentHealth;

        // Player dies if lost all health
        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }
    
    void OnHit(RayAndHit rayAndHit)
    {
        // Random damage
        isDamaged = true;
        int damagedValue = Random.Range(1, 5);

        // Reduce the current health by the damage amount
        currentHealth -= damagedValue;

        // Set the health bar's value to the current health
        healthSlider.value = currentHealth;

        // Player dies if lost all health
        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }
}