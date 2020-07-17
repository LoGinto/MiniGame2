using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIWeapon : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public AudioClip shootSound;
    public Transform rayPoint;
    RaycastHit hitInfo;
    Ray damRay;
    AudioSource audioSource;

    private void Awake()
    {
        Physics.queriesHitBackfaces = false;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        damRay = new Ray(rayPoint.position, transform.forward);
        Vector3 forward = rayPoint.TransformDirection(Vector3.forward) * 1;
        Debug.DrawRay(rayPoint.position, forward, Color.yellow);        
    }
    public void Shoot()
    {
        if (Physics.Raycast(damRay,out hitInfo,50))
        {
            if (hitInfo.collider.gameObject.tag == "AI"|| hitInfo.collider.gameObject.tag == "Player")
            {
                StartCoroutine(TryToKill());
            }
        }
    }
    IEnumerator TryToKill()
    {
        audioSource.PlayOneShot(shootSound);
        try
        {
            hitInfo.collider.GetComponent<Health>().TakeDamage(damage);
        }
        catch
        {
            Debug.Log(hitInfo.collider.name + " should take damage");
        }
        yield return new WaitForSeconds(fireRate);
    }

}
