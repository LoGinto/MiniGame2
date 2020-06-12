using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirForAI : MonoBehaviour
{
    public GameObject parachute;
    [SerializeField] private float rigidDrag = 2.5f;
    [SerializeField] Transform landTo;
    [SerializeField] GameObject destination;
    Rigidbody rigid;
    private float distanceToGround;
    Collider aiCollider;
    public float parachuteSpeed = 5f;
    private bool openedChute = false;
    private float randomOpenTime;
    private void Start()
    {
        parachute.gameObject.SetActive(false);
        aiCollider = GetComponent<Collider>();       
        distanceToGround = aiCollider.bounds.extents.y;
        randomOpenTime = Random.Range(2, 7);
        GameObject dest = Instantiate(destination);
        landTo = dest.transform;
        rigid = GetComponent<Rigidbody>();
    }
    public bool AiOnLand()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
    private void Update()
    {
        Parachuting();      
    }
    private void FixedUpdate()
    {
        TowardsTheRandPos();
    }
    void TowardsTheRandPos()
    {
        if (openedChute && AiOnLand() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position,landTo.position * parachuteSpeed * Time.deltaTime,1);
        }
    }
    private void Parachuting()
    {
        if(AiOnLand() == false)
        {
            rigid.useGravity = true;
        }
        if (!openedChute)
        {
            StartCoroutine(ParachuteOpen());
        }
        if (openedChute)
        {
            gameObject.GetComponent<Rigidbody>().drag = rigidDrag;
            if (AiOnLand() == true)
            {
                rigidDrag = 0;
                parachute.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator ParachuteOpen()
    {
        yield return new WaitForSeconds(randomOpenTime);
        openedChute = true;
        parachute.gameObject.SetActive(true);
    }
}
