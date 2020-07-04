using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonView : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;
    public GameObject cinemachineFreeLookToDisable;
    public int camMode = 1;
    bool instantiated = false;
    public bool isInFirstPersonMode = false;
    public GameObject weapon;
    public GameObject weaponPivot;
    public Transform fpsCam;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (isInFirstPersonMode)
        {
            fpsCam.gameObject.SetActive(true);
        }
        else
        {
            fpsCam.gameObject.SetActive(false);
        }
        
        if(weapon == null)
        {
            weapon = GameObject.FindGameObjectWithTag("Weapon");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(camMode == 1)
            {
                camMode = 0;
            }
            else
            {
                camMode += 1;
            }
            StartCoroutine(CameraChange());
        }
        if (isInFirstPersonMode == true)
        {
            weapon.transform.parent = fpsCam.transform;
        }
        else if(isInFirstPersonMode == false)
        {
            weapon.transform.parent = weaponPivot.transform;
        }
    }
    public void SetCamMode(int cam)
    {
        camMode = cam;
    }
    IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(0.01f);
        if(camMode == 0)
        {
            thirdPersonCamera.SetActive(false);
            firstPersonCamera.SetActive(true);
            isInFirstPersonMode = true;
            instantiated = true;
        }
        if(camMode == 1)
        {
            thirdPersonCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
            isInFirstPersonMode = false;
        }
    }
}
