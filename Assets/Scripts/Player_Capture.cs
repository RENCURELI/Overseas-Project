using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Capture : MonoBehaviour
{
    GameObject currentCapturer = null;
    GameObject potentialCapturer = null;

    bool isCapturable = true;
    string captureBy = "None";
    int captureTime = 0;
    [SerializeField] int captureTimeNeeded = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (currentCapturer == null)
        {
            currentCapturer = collision.gameObject;
        } else if (currentCapturer != collision.gameObject) {
            isCapturable = false;
            potentialCapturer = collision.gameObject;
        }
    }

    private void OnCollisionStay(Collision collision)
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

    private void OnCollisionExit(Collision collision)
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
