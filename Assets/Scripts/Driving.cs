using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    CarBase carBase;
    GameObject player;
    private bool entered;
    [SerializeField] float enteringDistance;
    [SerializeField] GameObject virtualCamToDisable;
    [SerializeField] GameObject mainCamToDisable;
    [SerializeField] Camera carCamera;
    AudioListener[] audioListener;

    private void Start()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
            entered = false;
            carBase = GetComponent<CarBase>();
            audioListener = GetComponentsInChildren<AudioListener>();
            foreach (AudioListener listener in audioListener)
            {
                listener.GetComponent<AudioListener>().enabled = false;
            }
        }
        catch
        {
            return;
        }
    }
    private void Update()
    {
        try
        {
            carBase.enabled = entered;
            carCamera.gameObject.SetActive(entered);
            player.SetActive(!entered);
            virtualCamToDisable.SetActive(!entered);
            mainCamToDisable.SetActive(!entered);
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (isOnSelectedDistanceToPlayer(enteringDistance))
                {
                    entered = !entered;
                }
            }
            if (entered)
            {
                player.transform.parent = gameObject.transform;
                foreach (AudioListener listener in audioListener)
                {
                    listener.GetComponent<AudioListener>().enabled = true;
                }
            }
            else
            {
                player.transform.parent = null;
                foreach (AudioListener listener in audioListener)
                {
                    listener.GetComponent<AudioListener>().enabled = false;
                }
            }
        }
        catch
        {
            return;
        }
    }
    private bool isOnSelectedDistanceToPlayer(float distance)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= distance;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enteringDistance);
    }
    public bool Entered()
    {
        return entered;
    }
}
