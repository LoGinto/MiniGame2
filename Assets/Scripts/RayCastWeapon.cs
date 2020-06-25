using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public Transform rayCastOrigin;
    public Transform rayCastDestination;
    public float weaponDamage;
    public TrailRenderer trail;
    Ray ray;
    RaycastHit hitInfo;
    Vector3 dest;
    Vector3 origin; 
    private void Start()
    {
        Physics.queriesHitBackfaces = false;
    }
    public void StartFiring()
    {
        isFiring = true;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        ray.origin = rayCastOrigin.position;
        ray.direction = rayCastDestination.position-rayCastOrigin.position;
        var tracer = Instantiate(trail,ray.origin,Quaternion.identity);
        tracer.AddPosition(ray.origin);
        dest = ray.direction;
        origin = ray.origin;
        if (Physics.Raycast(ray,out hitInfo))
        {
            if (hitInfo.collider.tag != "Enemy")
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);        
            }//otherwise I will apply flesh effect
            tracer.transform.position = hitInfo.point;
        }
        //if(hitInfo.collider.tag == "Enemy" || hitInfo.collider.tag == "AI")
        //{
        //    hitInfo.collider.GetComponent<Health>().TakeDamage(weaponDamage);                
        //}
    }
    public void StopFiring()
    {
        isFiring = false;
    }
    public Vector3 GetDestination()
    {
        return dest;
    }
    public Vector3 GetOrigin()
    {
        return origin;
    }
}
