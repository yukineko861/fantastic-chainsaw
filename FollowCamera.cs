﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 10.0f;
    private const float Y_ANGLE_MAX = 35.0f;
    private const float DISTANCE_MAX = 20.0f;
    private const float DISTANCE_MIN = 5.0f;
    private const float TRANS_MIN = 1.0f;
    private const float TRANS_MAX = 2.0f;

    public float dragSpeed = 1;
    private Vector3 dragOrigin;
    public Transform lookAt;
    public Transform camTransform;
    public GameObject player;
    private Camera cam;
    public float distance = 5.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 2.0f;
    private float trandis;
    public Vector3 height = new Vector3(0, 1, 0);
    private bool below = false;

    private void Start()
    {
        //Makes camTransform a transform.
        camTransform = transform;
        //Sets variable cam value to the main camera
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //Makes the camera move by looking at the axis of the mouse(Also multiplied by the seisitivity.)
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY += Input.GetAxis("Mouse Y") * sensitivityY;
        }       
        
        //Limits the Y variable
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        //Thiago Laranja's scrollwheel implemetation.
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { distance -= 1.0f; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { distance += 1.0f; }
        //Makes sure that these variables never go over the max and be les than the min.
        distance = Mathf.Clamp(distance, DISTANCE_MIN, DISTANCE_MAX);
        trandis = Mathf.Clamp(distance, TRANS_MIN, TRANS_MAX) - 1;
        //Sets players transparency(Make sure that player materials rendering mode has set to transparent or other mode that supports transparency).
        player.GetComponent<Renderer>().material.color = new Color(player.GetComponent<Renderer>().material.color.r, player.GetComponent<Renderer>().material.color.g, player.GetComponent<Renderer>().material.color.b, trandis);
        //Disables the object from rendering if your're at distance 2.
        if (distance <= 2f) { player.GetComponent<Renderer>().enabled = false; }
        if (distance > 2f) { player.GetComponent<Renderer>().enabled = true; }
        //If close enough to the character sinp into distance of 0.1(If distance is 0 the camera cant be rotated.)
        if (distance <= 0.8f && below == false) { distance = 0.1f; below = true; }
        if (distance >= 0.8f && below == true) { below = false; }
    }
    private void LateUpdate()
    {
        //Subtracts hte distance from Z coordinate
        Vector3 dir = new Vector3(0, 0, -distance);
        //Creates an quaternion for rotation(too bad that we cannot use Vector3.
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        //Sets the cameras position and makes it look at player.
        camTransform.position = lookAt.position + height + rotation * dir;
        camTransform.LookAt(lookAt.position + height);
        Vector3 angles = camTransform.eulerAngles; //stores rotation in a temporary variable
        angles.y = (angles.y / 90) * 90; // rounds rotation in 90's
        camTransform.eulerAngles = angles; // sets rotation to camera
    }
}
