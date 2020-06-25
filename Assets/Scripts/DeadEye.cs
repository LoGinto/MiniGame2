using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class DeadEye : MonoBehaviour
{
    RayCastWeapon weapon;
    public List<Transform> targets;
    Aiming aiming;
    public PostProcessVolume ppv;
    private float cooldownTimer;
    public Transform[] cross;
    public enum DeadEyeState
    {
        off,
        aiming,
        shooting
    };
    private DeadEyeState deadEyeState = DeadEyeState.off;
    private void Start()
    {
        weapon = GetComponentInChildren<RayCastWeapon>();
        aiming = GetComponent<Aiming>();
    }
    private void FixedUpdate()
    {
        UpdateState();
        UpdateTargetUI();
    }

    private void UpdateState()
    {
        if (deadEyeState == DeadEyeState.off)
        {
            aiming.enabled = true;
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (ppv.weight > 0)
                ppv.weight -= Time.deltaTime * 2;
        }
        else if (deadEyeState == DeadEyeState.shooting)
        {
            // Reset time
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            aiming.enabled = false;           
            UpdateDeadEye();
        }
        else
        {
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;          
            aiming.enabled = true;
            if (ppv.weight < 1)
                ppv.weight += Time.deltaTime * 2;
        }

    }
    private void UpdateTargetUI()
    {
        // Loop through all "cross" target indicators
        for (int i = 0; i < cross.Length; i++)
        {
            // If we are still within the targets we have
            if (i < targets.Count)
            {
                // Activate it
                cross[i].gameObject.SetActive(true);
                // Then update its position to the screen position of the target
                cross[i].position = Camera.main.WorldToScreenPoint(targets[i].position);
            }
            else // If we exceeded the last target
                cross[i].gameObject.SetActive(false); //Deactivate it
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (deadEyeState == DeadEyeState.off)
            {
                deadEyeState = DeadEyeState.aiming;
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (deadEyeState == DeadEyeState.off)
                aiming.aimLayer.weight = 1;
                weapon.StartFiring();
            if (deadEyeState == DeadEyeState.aiming)
            {
                //assigning
                RaycastHit hitInfo;
                if (Physics.Raycast(weapon.GetOrigin(),weapon.GetDestination(),out hitInfo))
                {
                    GameObject tmpTarget = new GameObject();
                    tmpTarget.transform.position = hitInfo.point;
                    tmpTarget.transform.parent = hitInfo.transform;
                    targets.Add(tmpTarget.transform);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.X))
        {         
            if (deadEyeState == DeadEyeState.aiming)
                deadEyeState = DeadEyeState.shooting;
        }
    }
    void UpdateDeadEye()
    {
        if(deadEyeState == DeadEyeState.shooting && targets.Count > 0)
        {
            Transform currentTarget = targets[0];
            Quaternion rot = Quaternion.LookRotation(currentTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 30 * Time.deltaTime);
            float diff = (transform.eulerAngles - rot.eulerAngles).magnitude;
            if (diff <= 0.1f && cooldownTimer <= 0)
            {
                aiming.aimLayer.weight = 1;
                weapon.StartFiring();

                // Remove the target form the list
                targets.Remove(currentTarget);
                // Destroy the target
                Destroy(currentTarget.gameObject);//do i have to?
            }
        }
        else 
            deadEyeState = DeadEyeState.off; // Reset the DeadEye state to off
    }

 }
                              

