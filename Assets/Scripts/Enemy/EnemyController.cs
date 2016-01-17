﻿using UnityEngine;
using System.Collections;

public class RayAndHit
{
    public Ray ray;
    public RaycastHit hit;
    public RayAndHit(Ray ray, RaycastHit hit)
    {
        this.ray = ray;
        this.hit = hit;
    }
}

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
            SetArsenal(arsenal[1].name);
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
    
    void OnHit(RayAndHit rayAndHit)
    {
        //Debug.Log("Hit");
        //Debug.Log(transform.tag);
        // Add a big force impact from the bullet hit
        //GetComponent<Rigidbody>().AddForce(rayAndHit.ray.direction * 0.1f, ForceMode.Impulse);
    }
}