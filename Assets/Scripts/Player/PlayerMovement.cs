using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public Transform upperBody;
    public Transform pivot;
    public OrbHealth orbHealth;
    public PlayerHealth playerHealth;
    public Gameflow gameFlow;

    Vector3 movement = new Vector3();
    Vector3 velocity = new Vector3();
    Quaternion startRot;
    Quaternion endRot;

    Animator atr;

    int counter = 0;
    int counter2 = 0;
    int counter3 = 0;
    int[] myIntArray = new int[15];
    //int[] myIntArray = new int[60];
    bool inWalk = false;
    bool inAttack = false;
    bool inAim = false;
    bool inTransition = false;
    bool fired = false;

    // prev: idle = 0, move = 1, attack = 2, aim = 3, transition = 4, take damage = 5 
    int prev = 0;

    float inAirMultiplier = 0.25f;
    float speed = 6f;
    
    void Start()
    {
        endRot = upperBody.rotation;
        atr = GetComponent<Animator>();
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!playerHealth.isDead)&&(!gameFlow.inPause))
        {
            if (playerHealth.isDamaged)
            {
                atr.CrossFade("Damaged", 0);
                atr.SetBool("Damaged", true);
                if (counter2 > 13)
                {
                    counter2 = 0;
                    playerHealth.isDamaged = false;
                }
                prev = 5;
                counter2++;
            }
            else
            {
                atr.SetBool("Damaged", false);
                
                //shoot if left mouse pressed
                if (Input.GetMouseButton(0))
                {
                    atr.CrossFade("Attack", 0);
                    atr.SetBool("Attack", true);
                    atr.SetBool("Aim", false);
                    atr.SetBool("Walk", false);
                    inAttack = true;

                    Shoot();

                    counter = 0;
                    prev = 2;

                }
                //walk if "WASD" pressed and not shooting
                else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && (!Input.GetMouseButton(0)))
                {
                    //check if previous updates included shooting 
                    if (Array.Exists(myIntArray, ContainsValue1))
                    {
                        //do nothing
                    }
                    else
                    {
                        if (prev != 1)
                        {
                            atr.CrossFade("Walk", 0);
                            atr.SetBool("Walk", true);

                        }

                        atr.SetBool("Attack", false);
                        atr.SetBool("Aim", false);

                        Walk();
                        prev = 1;
                    }
                }
                //return to aiming after shooting
                else if (prev == 2 || prev == 3)
                {
                    atr.SetBool("Walk", false);
                    atr.SetBool("Attack", false);
                    atr.SetBool("Aim", true);

                    if (counter > 30)
                    {
                        counter = 0;
                        prev = 4;
                    }
                    else
                        prev = 3;
                    counter++;
                }
                //transition state between aim and next state
                else if (prev == 4)
                {
                    atr.SetBool("Walk", false);
                    atr.SetBool("Attack", false);
                    atr.SetBool("Aim", false);
                    atr.CrossFade("Holster", 0);
                    if (counter > 45)
                    {
                        counter = 0;
                        prev = 0;
                    }
                    counter++;
                }
                //go to idle state if none other states satisfied
                else
                {
                    atr.SetBool("Walk", false);
                    atr.SetBool("Attack", false);
                    atr.SetBool("Aim", false);
                    atr.CrossFade("Idle", 0);
                    prev = 0;
                }
            }
        }
        else if (!gameFlow.inPause)
            atr.CrossFade("Damaged", 0);

        //update array to include previous shooting times
        if (Input.GetMouseButton(0))
        {
            updateIntArray(1);
        }
        else
            updateIntArray(0);

        counter3++;
    }

    //rotate player upper body up/down while shooting
    void LateUpdate()
    {
        if ((Input.GetMouseButton(0) || prev == 2 || prev == 3)&&(!playerHealth.isDead) && (!gameFlow.inPause))
        {
            Rotate(95);
            startRot = upperBody.rotation;
            fired = true;
        }
    }

    void Walk()
    {
        //determine player "WASD" rotation 
        var z = Input.GetKey(KeyCode.W) ? 0.0f : 0;
        z = Input.GetKey(KeyCode.S) ? 180.0f : z;

        var x = Input.GetKey(KeyCode.D) ? 90.0f : 0;
        x = Input.GetKey(KeyCode.A) ? 270.0f : x;

        movement.z = speed * z;
        movement.x = speed * x;

        //determine player "WD,SD,AS,AW" rotation
        var rotAngle = z + x;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            rotAngle = 45.0f;
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            rotAngle = 135.0f;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            rotAngle = 225.0f;
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            rotAngle = 315.0f;
        }
        Quaternion camRot = new Quaternion(player.rotation.x, cam.rotation.y, player.rotation.z, cam.rotation.w);
        player.rotation = camRot;
        player.Rotate(0, rotAngle, 0);
        //pivot.Rotate(0, -rotAngle, 0);

        //determine walking speed
        velocity.y += Physics.gravity.y * Time.deltaTime;
        movement.x *= inAirMultiplier;
        movement.z *= inAirMultiplier;

        movement += velocity;
        movement += Physics.gravity;
        movement *= Time.deltaTime;
        transform.GetComponent<CharacterController>().Move(transform.forward * speed * Time.deltaTime);
    }

    //rotate player left/right while shooting
    void Shoot()
    {
        Quaternion camRot_y = new Quaternion(player.rotation.x, cam.rotation.y, player.rotation.z, cam.rotation.w);
        player.rotation = camRot_y;
        //player.Rotate(0, 10, 0);
    }

    void Rotate(float angle)
    {
        float rotAngle = angle;
        float rotFactor = 0.6f;
        Quaternion camRot = new Quaternion(cam.rotation.x * rotFactor, upperBody.rotation.y, cam.rotation.z * rotFactor, upperBody.rotation.w);
        upperBody.rotation = camRot;
        upperBody.Rotate(-rotAngle, 0, 0);
    }

    void updateIntArray(int val)
    {
        if (counter3 > myIntArray.Length-1)
            counter3 = 0;
        myIntArray[counter3] = val;
    }

    private bool ContainsValue1(int val)
    {
        return val == 1;
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.collider.tag);

        //if (!hit.collider.tag.Contains("Laser") && ((!hit.collider.tag.Contains("Orb")) && (!orbHealth.isDead)))
        if (!hit.collider.tag.Contains("Laser")&& !hit.collider.tag.Contains("Floor"))
        {
            playerHealth.TakeDamage(100);
        }
    }
}
