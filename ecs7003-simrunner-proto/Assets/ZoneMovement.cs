﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMovement : MonoBehaviour
{

    private GameObject player;
    public float speed;
    private bool chasing = false;
    private float acceleration;
    public float initialDistance = 15f;
    // Update is called once per frame
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        //transform.position.z = player.transform.position.z - initialDistance;

    }
    void Update()
    {
        acceleration = player.GetComponent<PlayerController>().acceleration;
        if (chasing)
        {
            transform.position = transform.position + (Vector3.forward * acceleration * speed * Time.deltaTime);
        }
    }

    public void beginChase()
    {
        chasing = true;
    }

    public void pauseChase()
    {
        chasing = false;
    }
}

