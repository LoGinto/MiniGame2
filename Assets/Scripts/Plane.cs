using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{ 
    [SerializeField] GameObject prop;
    [SerializeField] GameObject virtualCamToDIsable;
    [SerializeField] Camera planeCam;
    [SerializeField] GameObject mainCamToDisable;
    [SerializeField] float planeSpeed;
    [SerializeField]GameObject player;
    private bool playerEjected = false;
    Locomotion locomotionScriptToDisable;
    Rigidbody playerRigid;
    Aiming aimScriptToDisable;
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerEjected = false;
        player.transform.parent = gameObject.transform;
        player.SetActive(false);
        locomotionScriptToDisable = player.GetComponent<Locomotion>();
        aimScriptToDisable = player.GetComponent<Aiming>();      
        aimScriptToDisable.enabled = false;
        locomotionScriptToDisable.enabled = false;      
        playerRigid = player.GetComponent<Rigidbody>();
        playerRigid.useGravity = false;
        mainCamToDisable.SetActive(false);
        planeCam.gameObject.SetActive(true);    
        virtualCamToDIsable.SetActive(false);
    }
    public void EjectPlayer()
    {
        playerRigid.useGravity = true;
        player.SetActive(true);
        planeCam.gameObject.SetActive(false);
        virtualCamToDIsable.SetActive(true);
        mainCamToDisable.SetActive(true);
        player.transform.parent = null;
        playerEjected = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&playerEjected == false)
        {
            EjectPlayer();
        }
        transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
        prop.transform.Rotate(new Vector3(0,0,180));
    }
   
    public Locomotion GetLocomotionScript()
    {
        return locomotionScriptToDisable;
    }
    public Aiming GetAimScript()
    {
        return aimScriptToDisable;
    }
    public bool HasEjected()
    {
        return playerEjected;
    }
}
