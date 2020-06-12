using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorDestinationFOrAiAir : MonoBehaviour
{
    public int xPos;
    public int zPos;
    [SerializeField] int xposRange1 = -2, xposRange2 = 50;
    [SerializeField] int zposRange1 = 350, zposRange2 = 300;
    private void Start()
    {
        xPos = Random.Range(xposRange1, xposRange2);
        zPos = Random.Range(zposRange1, zposRange2);
        this.gameObject.transform.position = new Vector3(xPos, 1.5f, zPos);
    }
}
