using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(NetworkAnimator))]
public class Player_Animator : MonoBehaviour
{
    NetworkAnimator NetAnim;
    // Start is called before the first frame update
    void Start()
    {
        NetAnim = GetComponent<NetworkAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        NetAnim.SetTrigger("Default");
    }
}
