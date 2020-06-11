using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landed : MonoBehaviour
{
    private float distanceToGround;
    [SerializeField] GameObject weapon;
    Collider playerCollider;
    void Start()
    {
        playerCollider = GetComponent<Collider>();
        distanceToGround = playerCollider.bounds.extents.y;
        GetComponent<Aiming>().enabled = false;
        weapon.SetActive(false);
        GetComponent<Locomotion>().enabled = false;
    }
    public bool IsOnLand()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
    private void Update()
    {
        if(IsOnLand() == true)
        {
            GetComponent<Aiming>().enabled = true;
            weapon.SetActive(true);
            Destroy(gameObject.GetComponent<Aerial>().GetParachuteObj(),0.5f);
            GetComponent<Locomotion>().enabled = true;
            gameObject.GetComponent<Landed>().enabled = false;
            gameObject.GetComponent<Aerial>().enabled = false;
        }
    }
}
