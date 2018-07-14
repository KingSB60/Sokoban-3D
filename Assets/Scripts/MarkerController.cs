using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    public float speed;

    private Vector3 movement;
    private Vector3 destination;

    // Use this for initialization
    void Awake()
    {
        movement = Vector3.up * 0.5f;
        destination = transform.localPosition + movement;

        //Debug.Log(indicatorMovement);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
            if (transform.localPosition == destination)
            {
                movement = -movement;
                destination = transform.localPosition + movement;
            }
        }
    }
}
