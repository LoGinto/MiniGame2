﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    //problem is stickin to wall
    [SerializeField] float distanceToTakeCover = 3f;
    [SerializeField] Transform rayCastPoint;
    [SerializeField] float rollSpeed = 3f;
    [SerializeField] float stickToCoverDist = 0.5f;
    [SerializeField] RuntimeAnimatorController coverController;
    [SerializeField] RuntimeAnimatorController normalController;
    [SerializeField] LayerMask coverLayer;
    [SerializeField] float moveInCoverSpeed;
    Collider playerCollider;
    Animator animator;
    Locomotion locomotion;
    Aiming aiming;
    private float initialCapsuleHeight;
    CapsuleCollider playerCapsule;
    private bool tookCover;
    private bool isInCover;
    GameObject hitObj;
    private void Start()
    {
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        locomotion = GetComponent<Locomotion>();
        aiming = GetComponent<Aiming>();
        tookCover = false;
        playerCapsule = GetComponent<CapsuleCollider>();
        initialCapsuleHeight = playerCapsule.height;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (tookCover == false)
                TakeCover();
            else
                ExitCover();
            aiming.enabled = !tookCover;
            locomotion.enabled = !tookCover;           
        }
        //if (isInCover)
        //{
        //    MoveInCover();
        //}

    }
   private  void TakeCover()
    {
        RaycastHit hitInfo;
        Vector3 fwd = rayCastPoint.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(rayCastPoint.position, fwd, out hitInfo, distanceToTakeCover, coverLayer))
        {
            if (hitInfo.collider.gameObject != null && tookCover == false)
            {
                tookCover = true;
                hitObj = hitInfo.collider.gameObject;               
                if (Vector3.Distance(transform.position,hitInfo.point)>=stickToCoverDist)
                {
                    //roll to cover
                    //got problem here
                    transform.position = Vector3.MoveTowards(transform.position, hitInfo.point, rollSpeed);
                }
                else 
                {
                    
                    animator.runtimeAnimatorController = coverController as RuntimeAnimatorController;
                    if (hitObj.GetComponent<Collider>().bounds.max.y >= playerCollider.bounds.max.y)
                    {
                        //do the stand anim  
                        animator.SetBool("Stand", true);
                        isInCover = true;
                        Debug.Log("I stand in cover ");                        
                    }
                    else if (hitObj.GetComponent<Collider>().bounds.max.y < playerCollider.bounds.max.y)
                    {
                        //do the crouch anim
                        animator.SetBool("Crouch", true);
                        playerCapsule.height = initialCapsuleHeight / 2;
                        isInCover = true;
                        Debug.Log("I crouch in cover " + isInCover);
                    }
                }
            }
        }
    }
    void ExitCover()
    {
        tookCover = false;
        isInCover = false;
        animator.runtimeAnimatorController = normalController;
        playerCapsule.height = initialCapsuleHeight;
    }
    //private void MoveInCover()
    //{
       
    //}

}
