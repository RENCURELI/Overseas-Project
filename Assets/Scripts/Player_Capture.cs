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
    Color captureBy;
    int captureTime = 0;
    [SerializeField] int captureTimeNeeded = 500;
    public Slider slider;
    public Text textOnCapture;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = captureTimeNeeded;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
            slider.transform.GetChild(1).GetComponentInChildren<Image>().color = currentCapturer.GetComponent<Player>().teamColor;
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
        collision.gameObject.GetComponent<Rigidbody>().WakeUp();
        if (isCapturable && (captureBy != currentCapturer.GetComponent<Player>().teamColor ))
        {
            captureTime++;
        }
        if (captureTime >= captureTimeNeeded)
        {
            captureBy = currentCapturer.GetComponent<Player>().teamColor;
            captureTime = 0;
            CaptureFeedback();
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

    void CaptureFeedback()
    {
        meshRenderer.material.color = captureBy;
        slider.transform.GetChild(0).GetComponent<Image>().color = captureBy;
        textOnCapture.text = "Capturé par " + currentCapturer.name;
        StartCoroutine(FadeTextToFullAlpha(1f, textOnCapture));
        StartCoroutine(FadeTextToZeroAlpha(1f, textOnCapture));
    }

   
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
 
    
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        yield return new WaitForSeconds(t+1f);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
