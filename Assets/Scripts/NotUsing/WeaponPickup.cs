using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] AnimatorOverrideController animatorOverride = null;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //do something
        }
    }
}
