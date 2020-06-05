using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class Aiming : MonoBehaviour
{
    public float turnSpeed = 15;
    Camera kamera;
    public Rig aimLayer;
    public float aimDuration = 0.3f;
    RayCastWeapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        kamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RayCastWeapon>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   
        {
            Cursor.visible = !Cursor.visible;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        float yawCam = kamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,yawCam,0),turnSpeed * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            aimLayer.weight += Time.deltaTime * aimDuration;
        }
        else
        {
            aimLayer.weight -= Time.deltaTime * aimDuration;
        }
        if (Input.GetButton("Fire1"))
        {
            weapon.StartFiring();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    }
}
