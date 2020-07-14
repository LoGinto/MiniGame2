using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class Behavior : MonoBehaviour
{
    //AI behavior
    [Header("Connected to wall vars")]
    public GameObject zoneWall;
    public LayerMask wallLayer;
    public float wallDetectionDistance = 50;  
    public float moveAwayFromWallMagnitudeMultiplier = 60;
    public Transform center;
    [Space(2)]
    [Header("Directly connected to state vars")]
    public LayerMask antoginizing;
    public enum StateMachine
    {
        calm,
        hostile,
        critical,
    };
    [Space(2)]
    [Header("Movement vars")]
    public float wanderRadius;
    public float wanderTime;
    public float walkSpeed;
    public float runSpeed;
    [Space(2)]
    [Header("Spotting and assigning vars")]
    public float fieldOfViewAngle = 110f;
    [SerializeField] Transform enemy;//will be assigned later
    //private variables
    private StateMachine state = StateMachine.calm;
    NavMeshAgent navigation;
    Vector3 direction;//look direction
    Animator animator;
    AudioSource audioSource;
    RaycastHit hitInfo;
    AirForAI air;
    Vector3 wallPos;
    bool enemyInSight;
    private float timer;
    SphereCollider sphereCollider;
    private bool closeToWall;
    private void Awake()
    {      
        Physics.queriesHitBackfaces = false;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        air = GetComponent<AirForAI>();
        sphereCollider = GetComponent<SphereCollider>();
        if (zoneWall == null)
        {
            zoneWall = GameObject.FindGameObjectWithTag("ZoneWall");
        }
        navigation = GetComponent<NavMeshAgent>() as NavMeshAgent;
        if (center == null)
        {
            center = GameObject.FindGameObjectWithTag("Center").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (air.AiOnLand() == true && navigation.isActiveAndEnabled)
        {
            StateAction(); 
            StateSwitch();
            CheckForWall();
        }              
    }

    private void CheckForWall()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, wallDetectionDistance, wallLayer, QueryTriggerInteraction.Collide);
        foreach (Collider detected in hitColliders)
        {
            if (detected.tag == "ZoneWall")
            {
                Debug.Log("Wall detected");
                
                wallPos = detected.transform.position;
                if (Vector3.Distance(transform.position, detected.transform.position) <= wallDetectionDistance + 20)
                {
                    closeToWall = false;
                }
                else
                {
                    closeToWall = true;
                }
            }           
        }
           
    }

    void StateSwitch()
    {
        //switching enums
        if(enemyInSight && state == StateMachine.calm)
        {
            state = StateMachine.hostile;
        }
        if(enemyInSight == false&& state == StateMachine.hostile)
        {
            state = StateMachine.calm;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (air.AiOnLand())
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("AI"))
            {
                enemyInSight = false;
            }
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (air.AiOnLand())
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("AI"))
            {
                enemyInSight = false;
                direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);//if enemy is within angle
                if (angle < fieldOfViewAngle * 0.5f)
                {
                    EnemySight();//basically look at enemy and recognize via raycast
                }
            }
        }
    }

    void StateAction()
    {
        if (state == StateMachine.calm)
        {
            Wander();
        }
        if (state == StateMachine.hostile)
        {
            Attack();
        }
        if (state == StateMachine.critical)
        {
            Evade();
        }
    }
    void Wander()
    {
        //walk around/explore but within wall
        if (!closeToWall)//causing trouble 
        {
            //wander
            Debug.Log(gameObject.name + "Wander");
            navigation.speed = walkSpeed;
            timer += Time.deltaTime;
            if (timer >= wanderTime)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                navigation.SetDestination(newPos);
                timer = 0;
            }
        }
        else//?
        {
            //go away from wall
            //Vector3 dir = transform.position - wallPos;
            //navigation.SetDestination(dir * moveAwayFromWallMagnitudeMultiplier);
            while (closeToWall == true)
            {
                navigation.speed = runSpeed;
                Debug.Log("Runaway");
                transform.LookAt(center);
                Vector3 dir = center.position - transform.position;
                dir.Normalize();
                navigation.SetDestination(dir * moveAwayFromWallMagnitudeMultiplier);
            }
        }

    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    void Attack()
    {
        //attack enemy
        transform.LookAt(Vector3.Scale(enemy.position, new Vector3(0, 1, 1)));
    }
    void Evade()
    {
        //run away or take medkit

    }
    
    void EnemySight()//assigning an enmy
    {
        
        if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hitInfo, sphereCollider.radius,antoginizing))
        {
            if (hitInfo.collider != null)
            {
                Debug.DrawLine(transform.position + transform.up, hitInfo.point, Color.red);
                if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("AI"))
                {
                    Debug.Log("I see player or AI");
                    enemy = hitInfo.transform;
                    enemyInSight = true;
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wallDetectionDistance);
    }
}
    
