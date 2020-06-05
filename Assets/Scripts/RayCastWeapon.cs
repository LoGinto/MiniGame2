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
    public TrailRenderer trail;
    Ray ray;
    RaycastHit hitInfo;
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

        if(Physics.Raycast(ray,out hitInfo))
        {
            if (hitInfo.collider.tag != "Enemy")
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);        
            }//otherwise I will apply flesh effect
            tracer.transform.position = hitInfo.point;
        }
    }
    public void StopFiring()
    {
        isFiring = false;
    }
}
