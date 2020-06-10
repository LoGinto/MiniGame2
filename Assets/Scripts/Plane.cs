using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField]GameObject disabledMesh1;
    [SerializeField]GameObject disabledMesh2;
    [SerializeField] GameObject weaponToDisable;
    [SerializeField] GameObject prop;
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
        locomotionScriptToDisable = player.GetComponent<Locomotion>();
        aimScriptToDisable = player.GetComponent<Aiming>();
        disabledMesh1.SetActive(false);
        aimScriptToDisable.enabled = false;
        locomotionScriptToDisable.enabled = false;
        disabledMesh2.SetActive(false);
        weaponToDisable.SetActive(false);
        playerRigid = player.GetComponent<Rigidbody>();
        playerRigid.useGravity = false;
    }
    public void EjectPlayer()
    {
        disabledMesh1.SetActive(true);
        disabledMesh2.SetActive(true);
        playerRigid.useGravity = true;
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
    public GameObject GetWeapon()
    {
        return weaponToDisable;
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
