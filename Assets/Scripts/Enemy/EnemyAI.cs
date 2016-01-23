using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public EnemyActions ea;
    public EnemyShoot es;
    private Transform player;                               
    public PlayerHealth playerHealth;                       
    private Transform soldier;                              
    public EnemyHealth soldierHealth;                       
    public Transform upperBody;

    float rotSpeed = 10.0f;                             // Soldier rotation speed
    float runSpeed = 10.0f;                             // Soldier running speed

    // Boolean states
    bool inHit = false;
    bool inAvoid = false;
    bool inAttack = false;
    bool inAim = false;
    bool inChase = false;

    Vector3 randomDirection;
    Vector3 start_pos;
    Vector3 end_pos;
    Quaternion start_rot;                               // Start rotation 
    Quaternion end_rot;                                 // End rotation
    Quaternion next_rot;                                // Next frame rotation
    int counter_shoot = 0;                              // Counter for Shoot()
    int counter_avoid = 0;                              // Counter for Avoid()
    int randTime = 60;
    float timer = 0.0f;                                 // General purpose timer
    int aimShotNum;                                     // Number of shots
    
    int prev;                                           // 0:attack 1:avoid 2:chase


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    

    void Update()
    {
        
        timer -= Time.deltaTime;  
        
        if (inAttack || inAvoid)
        {
            // Complete attack
            if (inAttack)
            {
                AimShot(Random.Range(2, 6));
            }
            // Complete avoid
            if (inAvoid)
            {
                Avoid();
            }
        }
        
        // Shoot if player in range
        else if (playerInRange())
        {
            if (inChase)
            {
                counter_shoot = 0;
                timer = 0.0f;
            }
            inChase = false;
            if (Random012())
            {
                AimShot(Random.Range(2, 6));
                prev = 0;
            }
            else if (prev == 0)
            {
                Avoid();
                prev = 1;
            }
        }
        // Chase player if not in range
        else if (!playerInRange() && !inAttack)
        {
            inChase = true;
            Chase();
            prev = 2;
        }
    }

    // Rotate upper body up/down while shooting
    void LateUpdate()
    {
        if (playerInRange())
        {
            Vector3 player_distance = player.position - transform.position;
            float distance = Mathf.Sqrt(player_distance.x * player_distance.x + player_distance.z * player_distance.z);
            float factor = -35;
            upperBody.Rotate(0, 0, factor*(1/distance));
        }
    }

    void AimShot(int numberOfShots)
    {
        // Face towards player
        end_rot = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, end_rot, Time.deltaTime * rotSpeed);

        if (!inAttack)
            aimShotNum = numberOfShots + 1;

        // Aim for few seconds
        if ((timer <= 0) && (counter_shoot == 0))
        {
            ea.Aiming();
            end_rot = Quaternion.LookRotation(player.position - transform.position);
            timer = 1.7f;
            counter_shoot++;
            inAttack = true;
        }

        // Shoot numberOfShots
        if ((timer <= 0) && (counter_shoot < aimShotNum) && (counter_shoot > 0))
        {
            timer = 0.5f;
            ea.Attack();
            es.Fire((player.position - transform.position), Quaternion.LookRotation(player.position - transform.position));
            counter_shoot++;
        }

        // Reload time after shooting
        if ((timer <= 0) && (counter_shoot == aimShotNum))
        {
            timer = 0.5f;
            counter_shoot++;
            ea.Stay();
        }

        // Return to stand position
        if ((timer <= 0) && (counter_shoot > aimShotNum))
        {
            timer = 0.5f;
            counter_shoot = 0;
            inAttack = false;
        }
    }

    // Face and move towards player 
    void Chase()
    {
        end_rot = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, end_rot, Time.deltaTime * rotSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
        ea.Run();
    }
    
    void Avoid()
    {
        // Rotate away from player for random time
        if (counter_avoid < randTime)
        {
            if (!inAvoid)
            {
                Quaternion player_rot = Quaternion.LookRotation(player.position - transform.position);
                float rand_rot = Random.Range(0.4f, 0.6f);
                if (Random01())
                {
                    next_rot = new Quaternion(transform.rotation.x, transform.rotation.y + rand_rot, transform.rotation.z, transform.rotation.w + rand_rot);
                }
                else
                {
                    next_rot = new Quaternion(transform.rotation.x, transform.rotation.y - rand_rot, transform.rotation.z, transform.rotation.w - rand_rot);
                }
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, next_rot, Time.deltaTime * rotSpeed);
            
            if (counter_avoid > randTime/2.5f)
            {
                transform.position = Vector3.LerpUnclamped(transform.position, transform.position + transform.forward, Time.deltaTime * runSpeed/2.0f);
            }

            ea.Run();
            counter_avoid++;
            inAvoid = true;
        }
        // Move in direction facing away from player for random time
        else if (counter_avoid < randTime*1.5f)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, transform.position + transform.forward, Time.deltaTime * runSpeed);
            ea.Run();
            counter_avoid++;
        }
        else
        {
            ea.Stay();
            counter_avoid = 0;
            inAvoid = false;
        }
    }
    
    void OnHit(RayAndHit rayAndHit)
    {
        inHit = true;  
    }

    // Detect if player is in range
    bool playerInRange()
    {
        Vector3 player_distance = player.position - transform.position;
        float distance = Mathf.Sqrt(player_distance.x * player_distance.x + player_distance.z * player_distance.z);
        return distance < 18;
    }

    // Randomly returns true or false
    bool Random01()
    {
        if (Random.Range(0, 2) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    // 80% true
    bool Random012()
    {
        int rand = Random.Range(0, 100);
        if (rand == 99)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    // Randomly return a number between "lower" and "upper"
    int randNum (int lower, int upper)
    {
        return Random.Range(lower, upper);
    }
}
