﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private LineRenderer lr;

	// Use this for initialization
	void Start ()
    {
        lr = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.up, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.collider.transform.position);
            }
        }
        else
            lr.SetPosition(1, transform.up * 5000);
    }
}
