using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    public float speed;

    private Vector3 movement;
    private Vector3 destination;

    // Use this for initialization
    void Start()
    {
        movement = Vector3.up * 0.5f;
        destination = transform.position + movement;

        //Debug.Log(indicatorMovement);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (transform.position == destination)
        {
            movement = -movement;
            destination = transform.position + movement;
        }
    }
}
