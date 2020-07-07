using System.Collections;
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
    FirstPersonView fps;
    Locomotion locomotion;
    private bool crouched;
    private bool stands;
    //temporary solution is to turn off dead eye
    DeadEye deadEye;
    //
    Vector3 tangent;
    Aiming aiming;
    Rigidbody rigid;
    private float initialCapsuleHeight;
    CapsuleCollider playerCapsule;
    private bool tookCover;
    float axis;
    private bool isInCover;
    public bool facingRight = true;
    GameObject hitObj;
    RaycastHit hitInfo;
    bool movin;
    Vector3 initialCenter;
    float old_pos;
    private void Start()
    {        
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        locomotion = GetComponent<Locomotion>();
        aiming = GetComponent<Aiming>();
        tookCover = false;
        old_pos = transform.position.x; 
        rigid = GetComponent<Rigidbody>();
        deadEye = GetComponent<DeadEye>();
        playerCapsule = GetComponent<CapsuleCollider>();
        initialCapsuleHeight = playerCapsule.height;
        initialCenter = playerCapsule.center;
    }
    private void Update()
    {
        if (isInCover)
        {
            movin = CheckForMovement();
        }
        //if (tookCover)
        //{
        //    fps.SetCamMode(1);               
        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (tookCover == false)
                TakeCover();
            else
                ExitCover();
            aiming.enabled = !tookCover;
            locomotion.enabled = !tookCover;
            deadEye.enabled = !tookCover;            
        }
        if (isInCover)
        {
            MoveInCover();
            //while facing right there is a problem, so I need to reset it afterwards
            Regulate();
        }
    }
    private void FixedUpdate()
    {
        if (isInCover)           
        {
            CheckForMovement();
        }
    }
    private void Regulate()
    {
        //condition to flip the bool
        
        if (axis < 0)
        {
            facingRight = false;            
        }
        if (axis > 0)
        {
            facingRight = true;           
        }       
        if (!facingRight && crouched) //then I have to check wheter I am moving or not
        {
            LeftFlip();
            
            if (movin)
            {
                Debug.Log("Left move anim");
               
            }
            else
            {
                
            }
            //will tweak these
        }
        if (facingRight && crouched)
        {
            RightFlip();
            
            if (movin)
            {
                Debug.Log("Right move anim");
                //animator.SetBool("MovingRight", true);
            }
            else
            {
               // animator.SetBool("MovingRight", false);
            }
        }
        
    }

    private void RightFlip()
    {
        animator.SetBool("LeftCrouch", false);
        transform.rotation = Quaternion.FromToRotation(tangent, hitInfo.normal);
        playerCapsule.center = new Vector3(0, 0.82f, 0);
        //0.71 collider height here or center?
        playerCapsule.height = 0.99f;
        //will tweak these
    }

    private void LeftFlip()
    {
        animator.SetBool("LeftCrouch", true);
        transform.rotation = Quaternion.FromToRotation(-tangent, hitInfo.normal);
        playerCapsule.center = new Vector3(0, 0.94f, 0);
        playerCapsule.height = 0.99f;
    }

    private  void TakeCover()
    {   
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
                    //got problem here but nothing too big
                    transform.position = Vector3.MoveTowards(transform.position, hitInfo.point, rollSpeed);
                }
                else 
                {
                    
                    animator.runtimeAnimatorController = coverController as RuntimeAnimatorController;
                    if (hitObj.GetComponent<Collider>().bounds.max.y >= playerCollider.bounds.max.y)
                    {
                        //do the stand anim  
                        //animator.SetBool("Stand", true);
                        isInCover = true;
                        crouched = false;
                        stands = true;
                        Debug.Log("I stand in cover ");                        
                    }
                    else if (hitObj.GetComponent<Collider>().bounds.max.y < playerCollider.bounds.max.y)
                    {
                        //do the crouch anim
                       // animator.SetBool("MovingRight", false);
                        animator.SetTrigger("TakeCover");
                        crouched = true;
                        stands = false;
                        //0.48 y center
                        playerCapsule.center = new Vector3(0, 0.48f, 0);
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
        playerCapsule.center = initialCenter;
    }
    private void MoveInCover()
    {
        Vector3 normal = hitInfo.normal;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        Vector3 t1 = Vector3.Cross(normal, Vector3.forward);
        Vector3 t2 = Vector3.Cross(normal, Vector3.up);
        if (t1.magnitude > t2.magnitude)
        {
            tangent = t1;
        }
        else
        {
            tangent = t2;
        }
        axis = Input.GetAxis("Horizontal");
        Vector3 moveDirection = axis * tangent;       
        transform.Translate(moveDirection * moveInCoverSpeed,Space.World);       
    }
    private bool CheckForMovement()
    {       
        if(old_pos < transform.position.x || old_pos > transform.position.x)
        {
            old_pos = transform.position.x;
            return true;
        }
        else
        {
            old_pos = transform.position.x;
            return false;
        }
        
    }
 
}
