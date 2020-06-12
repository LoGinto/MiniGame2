using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerial : MonoBehaviour
{
    [SerializeField] private float drag = 2f;
    public GameObject parachute;
    [SerializeField] float paraChuteControlSpeed = 2f;
    private bool opened;
    private Vector3 moveVector;
    Camera kamera;
    Landed landed;
    // Start is called before the first frame update
    void Start()
    {
        kamera = Camera.main;
        landed = GetComponent<Landed>();
    }
    // Update is called once per frame
    void Update()
    {
        if(landed.IsOnLand() == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!opened)
                {
                    parachute.SetActive(true);
                    opened = true;  
                }
               
                //inser parachute controls here
            }
            if (opened)
            {
                gameObject.GetComponent<Rigidbody>().drag = drag;
                if (landed.IsOnLand())
                {
                    drag = 0;
                }
            }
            //insert falling here

        }
        else
        {
            if (landed.IsOnLand() && opened)
            {
                //Destroy(parachute.gameObject, 0.5f);
                this.gameObject.GetComponent<Aerial>().enabled = false;  
            }
        }
    }
    private void FixedUpdate()
    {
        if (opened)
        {
            Vector3 cameraForward = Vector3.Scale(kamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            moveVector.x = Input.GetAxis("Horizontal") * cameraForward.x * paraChuteControlSpeed;
            moveVector.z = Input.GetAxis("Vertical") * cameraForward.z* paraChuteControlSpeed;
            moveVector.y = 0;
            moveVector.Normalize();
            moveVector *= paraChuteControlSpeed;
            transform.Translate(moveVector * Time.deltaTime);
        }
    }
    public GameObject GetParachuteObj()
    {
        return parachute;
    }
}
