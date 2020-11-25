﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    public float upForce;
    public float rightForce;

    public Transform body;
    Vector3 rot;

    private float distanceFromLeft;
    private float distanceFromRight;

    private float raycastLimit = 10f;

    [SerializeField] private bool onLeftWall;
    [SerializeField] private bool onRightWall;
    [SerializeField] private bool isWallRunning;

    private void Awake()
    {
        rot = GetComponent<Rigidbody>().transform.eulerAngles;
    }

    private void checkWall()
    {

        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);
        
        RaycastHit leftW;
        RaycastHit rightW;

        if (Physics.Raycast(rayLeft, out leftW, raycastLimit))
        {
            distanceFromLeft = Vector3.Distance(transform.position, leftW.point);
            if (distanceFromLeft < 5f)
            {
                onLeftWall = true;
                onRightWall = false;
                
            }
        }


        if (Physics.Raycast(rayRight, out rightW, raycastLimit))
        {
            distanceFromRight = Vector3.Distance(transform.position, rightW.point);
            if (distanceFromRight < 5f)
            {
                onLeftWall = false;
                onRightWall = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        checkWall();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Tiles") 
            || collision.transform.CompareTag("Speed Up Wall") 
            || collision.transform.CompareTag("Slow Down Wall")
            || collision.transform.CompareTag("Wall Break") 
            || collision.transform.CompareTag("Double Jump Wall"))
        {
            isWallRunning = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("Tiles") 
            || collision.transform.CompareTag("Speed Up Wall") 
            || collision.transform.CompareTag("Slow Down Wall")
            || collision.transform.CompareTag("Wall Break") 
            || collision.transform.CompareTag("Double Jump Wall"))
        {

            //transform.rotation = Quaternion.FromToRotation(GetComponent<Rigidbody>().transform.eulerAngles, collision.contacts[0].normal);
            if (onLeftWall) {
                GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce(transform.right * rightForce, ForceMode.Impulse);
            }

            if (onRightWall)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce(-transform.right * rightForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Tiles") 
            && collision.transform.CompareTag("Speed Up Wall") 
            && collision.transform.CompareTag("Slow Down Wall")
            && collision.transform.CompareTag("Wall Break") 
            && collision.transform.CompareTag("Double Jump Wall"))
        { 
            isWallRunning = false;
            //transform.rotation = Quaternion.FromToRotation(GetComponent<Rigidbody>().transform.eulerAngles, rot);
        } 
    }
}