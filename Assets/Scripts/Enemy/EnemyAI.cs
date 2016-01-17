using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private EnemySight enemySight;                          // Reference to the EnemySight script.
    public EnemyActions ea;
    public EnemyShoot es;

    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
    private Transform player;                               // Reference to the player's transform.
    public PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
    private Transform soldier;                               // Reference to the soldier's transform.
    public EnemyHealth soldierHealth;                      // Reference to the SoldierHealth script.
    
    //private LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
    public Transform upperBody;

   
    float attackSpeed = 0.1f;
    float rotSpeed = 10.0f;
    float runSpeed = 10.0f;
    float cooldown;

    bool inHit = false;
    bool inAvoid = false;
    bool inAttack = false;
    bool inAim = false;
    bool inChase = false;

    Vector3 randomDirection;
    Vector3 start_pos;
    Vector3 end_pos;
    Quaternion start_rot;
    Quaternion end_rot;
    Quaternion next_rot;
    int counter_shoot = 0;
    int counter = 0;
    int randTime = 60;
    float timer = 0.0f;
    int aimShotNum;
    //0:attack 1:avoid 2:chase
    int prev;
    

    void Awake()
    {
        //health
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        //soldier = GameObject.FindGameObjectWithTag("Soldier").transform;
        //soldierHealth = soldier.GetComponent<EnemyHealth>();

        // Setting up the references.
        //enemySight = GetComponent<EnemySight>();
        //nav = GetComponent<NavMeshAgent>();
        //lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }
    

    void Update()
    {
        // general purpose timer
        timer -= Time.deltaTime;  
        
        if (inAttack || inAvoid)
        {
            //complete attack
            if (inAttack)
            {
                AimShot(Random.Range(2, 6));
            }
                   
            //complete avoid
            if (inAvoid)
            {
                Avoid();
            }
        }
        
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
            
        //chase player if not in range
        else if (!playerInRange() && !inAttack)
        {
            inChase = true;
            Chase();
            prev = 2;
        }
    }

    //rotate player upper body up/down while shooting
    void LateUpdate()
    {
        //if (inAim || inAttack)
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
        //face towards player
        end_rot = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, end_rot, Time.deltaTime * rotSpeed);

        if (!inAttack)
            aimShotNum = numberOfShots + 1;

        //aim for few seconds
        if ((timer <= 0) && (counter_shoot == 0))
        {
            ea.Aiming();
            end_rot = Quaternion.LookRotation(player.position - transform.position);
            timer = 1.7f;
            counter_shoot++;
            inAttack = true;
        }

        //shoot three times
        if ((timer <= 0) && (counter_shoot < aimShotNum) && (counter_shoot > 0))
        {
            timer = 0.5f;
            ea.Attack();
            es.Fire((player.position - transform.position), Quaternion.LookRotation(player.position - transform.position));
            counter_shoot++;
        }

        //reload time after shooting
        if ((timer <= 0) && (counter_shoot == aimShotNum))
        {
            timer = 0.5f;
            counter_shoot++;
            ea.Stay();
        }

        //return to stand position
        if ((timer <= 0) && (counter_shoot > aimShotNum))
        {
            timer = 0.5f;
            counter_shoot = 0;
            inAttack = false;
        }
    }


    void Chase()
    {
        end_rot = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, end_rot, Time.deltaTime * rotSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
        ea.Run();
    }
    

    void Avoid()
    {
        if (counter < randTime)
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
            
            if (counter > randTime/2.5f)
            {
                transform.position = Vector3.LerpUnclamped(transform.position, transform.position + transform.forward, Time.deltaTime * runSpeed/2.0f);
            }

            ea.Run();
            counter++;
            inAvoid = true;
        }
        
        else if (counter < randTime*1.5f)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, transform.position + transform.forward, Time.deltaTime * runSpeed);
            ea.Run();
            counter++;
        }
        else
        {
            ea.Stay();
            counter = 0;
            inAvoid = false;
        }
    }
    
    void OnHit(RayAndHit rayAndHit)
    {
        inHit = true;  
    }

    //detect if player is in range
    bool playerInRange()
    {
        Vector3 player_distance = player.position - transform.position;
        float distance = Mathf.Sqrt(player_distance.x * player_distance.x + player_distance.z * player_distance.z);
        return distance < 18;
    }

    //randomly returns true or false
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

    //80% true
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

    //randomly return a number between "lower" and "upper"
    int randNum (int lower, int upper)
    {
        return Random.Range(lower, upper);
    }
}
