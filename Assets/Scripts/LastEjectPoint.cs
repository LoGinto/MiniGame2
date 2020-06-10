using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEjectPoint : MonoBehaviour
{
    [SerializeField] GameObject plane;
    Plane planeScript;
    private void Start()
    {
        planeScript = plane.GetComponent<Plane>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == plane)
        {
            if(planeScript.HasEjected() == false)
            {
                planeScript.EjectPlayer();
                Destroy(gameObject,0.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
