using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public Camera mycamera;
    private bool isRunning = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = !isRunning;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }
    private void Move()
    {
        Camera kamera = mycamera;
        float verticalAxis = Input.GetAxis("Vertical");//varies between 1 and -1
        float horizontalAxis = Input.GetAxis("Horizontal");
        //create a new vector
        Vector3 cameraForward = Vector3.Scale(kamera.transform.forward, new Vector3(1, 0, 1)).normalized; // forward direction in 2d

        Vector3 updatedVector = verticalAxis * cameraForward + horizontalAxis * kamera.transform.right;
        transform.LookAt(updatedVector + transform.position);//facing towards vector
        if (!isRunning)
        {
            animator.SetBool("Walk", true);//setting animator boolean
            transform.Translate(updatedVector * speed * Time.deltaTime, Space.World);//actual movement
        }
        else
        {
            animator.SetBool("Run", true);//setting animator boolean
            transform.Translate(updatedVector * runSpeed * Time.deltaTime, Space.World);//actual movement
        }
        if (verticalAxis == 0 && horizontalAxis == 0)//if player doesn't move
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        if (verticalAxis != 0 && horizontalAxis != 0 && !isRunning)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
        }
    }
}
