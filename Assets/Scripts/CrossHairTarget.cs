using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera kamera;
    Ray ray;
    RaycastHit hitInfo;
    private void Start()
    {
        kamera = Camera.main;
    }
    private void Update()
    {
        ray.origin = kamera.transform.position;
        ray.direction = kamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
