using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player_Capture : MonoBehaviour
{
    GameObject currentCapturer = null;
    GameObject potentialCapturer = null;
    

    bool isCapturable = true;
    string captureBy = "None";
    int captureTime = 0;
    [SerializeField] int captureTimeNeeded = 500;
    public Slider slider;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (captureBy == "1")
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            slider.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        } else if (captureBy == "2")
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            slider.transform.GetChild(0).GetComponent<Image>().color = Color.blue;
        }
    }


    void FixedUpdate()
    {
        if (currentCapturer != null)
        {
            if (currentCapturer.GetComponent<NetworkIdentity>().netId.ToString() == "1")
            {
                slider.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.red;
            }
            else
            {
                slider.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.blue;
            }
        }

        slider.value = captureTime;
    }

    void OnCollisionEnter(Collision collision)
    {
       if (currentCapturer == null)
        {
            currentCapturer = collision.gameObject;
        } else if (currentCapturer != collision.gameObject) {
            isCapturable = false;
            potentialCapturer = collision.gameObject;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (isCapturable && (captureBy != currentCapturer.GetComponent<NetworkIdentity>().netId.ToString() ))
        {
            captureTime++;
        }
        if (captureTime >= captureTimeNeeded)
        {
            captureBy = currentCapturer.GetComponent<NetworkIdentity>().netId.ToString();
            Debug.Log("Capturé par : "+currentCapturer.name+" : "+captureBy);
            captureTime = 0;  
        }
    }

    void OnCollisionExit(Collision collision)
    {
       if (collision.gameObject == currentCapturer)
        {
            currentCapturer = potentialCapturer;
        }
        potentialCapturer = null;
        isCapturable = true;
        captureTime = 0;
    }
}
