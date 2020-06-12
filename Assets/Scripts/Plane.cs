using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{ 
    [Header("Cams to enable/disable")]
    [SerializeField] GameObject virtualCamToDIsable;
    [SerializeField] Camera planeCam;
    [SerializeField] GameObject mainCamToDisable;
    [Space(2)]
    [Header("Plane vars")]
    [SerializeField] GameObject prop;
    [SerializeField] float planeSpeed;
    [Space(1)]
    [Header("Player and AI vars")]
    [SerializeField]GameObject player;
    [SerializeField] GameObject AI1;
    [SerializeField] Transform aiInstantiatePlace;
    [SerializeField] int aiAmount = 3;
    int aiCount = 0;
    private bool playerEjected = false;
    Locomotion locomotionScriptToDisable;
    Rigidbody playerRigid;
    Aiming aimScriptToDisable;
    private void Start()
    {
        aiCount = 0;
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
    bool spawnTrigger;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&playerEjected == false)
        {
            EjectPlayer();
        }
        if (spawnTrigger && aiCount<=aiAmount)
        {
            EjectAI();
        }
        StartCoroutine(OneMoreEjectNumerator());
        transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
        prop.transform.Rotate(new Vector3(0,0,180));
    }
    void EjectAI()
    {
        spawnTrigger = false;
        Instantiate(AI1,aiInstantiatePlace.position,Quaternion.identity);
        aiCount += 1;      
    }
    IEnumerator AIEjectNumerator()
    {    
      float randEjectTime = Random.Range(10, 60);
      yield return new WaitForSeconds(randEjectTime);
      spawnTrigger = true;
    }
    IEnumerator OneMoreEjectNumerator()
    {
        float randEjectTime = Random.Range(10, 60);
        yield return new WaitForSeconds(randEjectTime);
        StartCoroutine(AIEjectNumerator());
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
