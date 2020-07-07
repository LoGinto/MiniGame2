using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingWall : MonoBehaviour
{
    public float delayTillShrink;
    AudioSource audioSource;
    public AudioClip warningSound;
    public float shrinkFactor;
    public Vector3 minSize;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Shrink());
    }
    IEnumerator Shrink()
    {
        while (minSize.x < gameObject.transform.localScale.x && minSize.z < gameObject.transform.localScale.z)
        {
            this.gameObject.transform.localScale -= new Vector3(shrinkFactor, 0, shrinkFactor) * Time.deltaTime;
            yield return new WaitForSeconds(delayTillShrink);
            audioSource.PlayOneShot(warningSound);
        }              
    }

    private void OnTriggerExit(Collider other)
    {
        //to do
        //decrease timer then explode
    }
    private void OnTriggerEnter(Collider other)
    {
        //to do
        //reset exploding timer again
    }
}
