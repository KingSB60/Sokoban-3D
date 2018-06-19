using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour {

    //public Material normalMaterial;
    //public Material onGoalMaterial;
    public GameObject OnGoalIndicator;

    private float speed;
    private Vector3 movingDestination, lastPosition;
    private bool moving;
    private bool destinationIsGoal, destinationWasGoal;
    private PlayerController playerScript;
    private GameObject indicatorInstance;

    // Use this for initialization
    void Start ()
    {
        moving = false;
        destinationIsGoal = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (indicatorInstance!= null)
        {
        }
    }

    void LateUpdate ()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, movingDestination, speed * Time.deltaTime);
            moving = !transform.position.Equals(movingDestination);
            if (!moving)
                SetOnGoal();
        }
    }

    private void SetOnGoal()
    {
        //Debug.Log("destinationIsGoal = " + destinationIsGoal.ToString());

        //GetComponent<MeshRenderer>().material = destinationIsGoal ? onGoalMaterial : normalMaterial;
        if (destinationIsGoal)
        {
            var x = transform.position.x;
            var y = OnGoalIndicator.transform.position.y;
            var z = transform.position.z;
            var pos = new Vector3(x, y, z);
            indicatorInstance = Instantiate(OnGoalIndicator, pos, Quaternion.identity);
        }
        else if (indicatorInstance != null)
        {
            Destroy(indicatorInstance);
            indicatorInstance = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("OnTriggerEnter!");
        //print(other.gameObject);
        GameObject whoTouchedMe = other.gameObject;
        //Debug.Log("other object tagged with " + whoTouchedMe.tag);

        switch (whoTouchedMe.tag)
        {
            case "Player":
                //Debug.Log("touched by Player!");
                playerScript = whoTouchedMe.GetComponent<PlayerController>();

                speed = playerScript.speed;
                var movingDirection = playerScript.moveDirection;
                movingDestination = transform.position + movingDirection;
                lastPosition = transform.position;

                moving = true;
                playerScript.PushCount++;
                break;
            case "Floor":
                SetIsGoal(false);
                break;
            case "Goal":
                SetIsGoal(true);
                break;
            case "Wall":
            case "Chest":
                movingDestination = lastPosition;
                playerScript.MoveBack(true);
                break;
        }
    }

    private void SetIsGoal(bool value)
    {
        //Debug.Log("SetIsGoal --> " + value.ToString());

        if (destinationWasGoal != value)
        {
            if (value)
                playerScript.chestsOnGoal++;
            else
                playerScript.chestsOnGoal--;
        }

        destinationWasGoal = destinationIsGoal;
        destinationIsGoal = value;
    }
}
