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
    private MeshRenderer meshRenderer;

    //A changer avec le système de couleur
    Player[] players = new Player[2];

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = captureTimeNeeded;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //A changer avec le système de couleur
        if (players[0] != null && captureBy == players[0].netId.ToString())
        {
            meshRenderer.material.color = Color.red;
            slider.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        } else if (players[1] != null && captureBy == players[1].netId.ToString())
        {
            meshRenderer.material.color = Color.blue;
            slider.transform.GetChild(0).GetComponent<Image>().color = Color.blue;
        }

        if (currentCapturer != null && currentCapturer.GetComponent<Rigidbody>().IsSleeping())
        {
            currentCapturer.GetComponent<Rigidbody>().WakeUp();
        }
      
    }


    void FixedUpdate()
    {
        //A changer avec le système de couleur
        if (currentCapturer != null)
        {
            if (currentCapturer.GetComponent<NetworkIdentity>().netId.ToString() == players[0].netId.ToString())
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
        if (players[0] == null)
        {
            players[0] = collision.gameObject.GetComponent<Player>();
        } else if (players[1] == null && collision.gameObject.GetComponent<Player>() != players[0])
        {
            players[1] = collision.gameObject.GetComponent<Player>();
        } 
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
        collision.gameObject.GetComponent<Rigidbody>().WakeUp();
        if (isCapturable && (captureBy != currentCapturer.GetComponent<NetworkIdentity>().netId.ToString() ))
        {
            captureTime++;
        }
        if (captureTime >= captureTimeNeeded)
        {
            captureBy = currentCapturer.GetComponent<NetworkIdentity>().netId.ToString();
            Debug.Log("Capturé par : "+currentCapturer.name);
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
