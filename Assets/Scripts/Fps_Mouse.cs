using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fps_Mouse : MonoBehaviour
{
    public Vector2 mouseLook;
    public Vector2 smoothV;
    public float sensitivity;
    public float smoothing;
    public GameObject weaponPivot;
    public GameObject player;
    Camera kamera;
    public float turnSpeed = 15;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (weaponPivot == null)
        {
            weaponPivot = GameObject.FindGameObjectWithTag("WeaponPivot");
        }
        kamera = GetComponent<Camera>();
    }
    void Update()
    {
        Vector2 vct2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        smoothV.x = Mathf.Lerp(smoothV.x, vct2.x, 1 / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, vct2.y, 1 / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        //character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

    }
    private void FixedUpdate()
    {
        float cam = kamera.transform.rotation.eulerAngles.y;
        weaponPivot.transform.localRotation = Quaternion.Euler(0, mouseLook.x, 0);
        player.transform.localRotation = Quaternion.Euler(0, mouseLook.x, 0);
        //Quaternion.Slerp(character.transform.rotation, Quaternion.Euler(0, cam, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
    //public float mouseSpeed = 3;
    //public Transform player;
    //public Camera yourCam;

    //private void Update()
    //{
    //    float X = Input.GetAxis("Mouse X") * mouseSpeed;
    //    float Y = Input.GetAxis("Mouse Y") * mouseSpeed;

    //    player.Rotate(0, X , 0);
    //    if (yourCam.transform.eulerAngles.x + (-Y) > 80 && yourCam.transform.eulerAngles.x + (-Y) < 280)
    //    { }
    //    else
    //    {

    //        yourCam.transform.RotateAround(player.position, yourCam.transform.right, -Y);
    //    }


