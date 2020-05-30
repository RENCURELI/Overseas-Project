﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public float lifeDuration = 2f;
    private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
            
        }
    }

}
