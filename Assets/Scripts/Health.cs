using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour
{
    public float hp;
    public GameObject popUpObject;
    List<GameObject> popUpList;
    public Camera kamera;
    bool hurt = false;
    void Start()
    {
        kamera = Camera.main;
        popUpList = new List<GameObject>();
    }
    public void TakeDamage(float damage)
    {
            hp -= damage;
            hurt = true;
            GameObject clone;
            clone = Instantiate(popUpObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            clone.transform.GetComponent<TextMeshPro>().text = damage.ToString();
            clone.transform.GetComponent<TextMeshPro>().color = Color.yellow;
            clone.transform.GetComponent<TextMeshPro>().fontSize = 5;
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                clone.transform.GetComponent<TextMeshPro>().alignment = TMPro.TextAlignmentOptions.Center;
            }
            else if (rand == 1)
            {
                clone.transform.GetComponent<TextMeshPro>().alignment = TMPro.TextAlignmentOptions.Left;
            }
            else if (rand == 2)
            {
                clone.transform.GetComponent<TextMeshPro>().alignment = TMPro.TextAlignmentOptions.Right;
            }

            //Add Clone To List And Start The Wait Coroutine
            popUpList.Add(clone);
            StartCoroutine(Wait(clone));
        if (hp <= 0)
        {
            Die();
        }
    }
    IEnumerator Wait(GameObject clone)
    {
        yield return new WaitForSeconds(.5f);
        popUpList.Remove(clone);
        Destroy(clone);
    }
    private void Update()
    {
        foreach (GameObject pop in popUpList)
        {
            //Update Popup To Remain Over Character Even While Moving
            pop.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);           
            pop.transform.GetComponent<TextMeshPro>().fontSize = pop.transform.GetComponent<TextMeshPro>().fontSize - Time.deltaTime * 5;      
            //Use The LookRotation Function To Make Sure That The Popup Always Faces The Main Camera
            pop.transform.rotation = Quaternion.LookRotation(kamera.transform.forward);    
        } 
        if(kamera == null)
        {
            try
            {
                kamera = Camera.main;
            }
            catch
            {
                try
                {
                    GameObject fpsobj = GameObject.FindGameObjectWithTag("Fps_Cam");
                    kamera = fpsobj.GetComponent<Camera>();
                }
                catch
                {
                    return; 
                }
            }
        }
    }
    void Die()
    {
        Debug.Log(gameObject.name + " is dead");//for now
        Destroy(gameObject);//for now
    }
    public bool GetPain()
    {
        return hurt;
    }
    public void SetPain(bool value)
    {
        hurt = value;
    }
    
}
