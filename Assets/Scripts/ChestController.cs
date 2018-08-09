using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour {

    //public Material normalMaterial;
    //public Material onGoalMaterial;
    public GameObject OnGoalIndicator;
    public GameObject levelCompleted;
    public Material defaultMaterial;
    public Material glowMaterial;

    private float speed;
    //private Vector3 movingDestination, lastPosition;
    //private bool moving;
    private bool _destinationIsGoal;//, destinationWasGoal;
    private PlayerController playerScript;
    //private GameObject indicatorInstance;
    private GameManager gameManager;

    private bool DestinationIsGoal
    {
        get { return _destinationIsGoal; }
        set
        {
            _destinationIsGoal = value;
            //transform.Find("OnGoalMarker").gameObject.SetActive(value);

            GameObject chest = transform.Find("large_crate").gameObject;
            Renderer rend = chest.GetComponent<Renderer>();
            rend.material = value ? glowMaterial : defaultMaterial;
        }
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    // Use this for initialization
    void Start ()
    {
        //moving = false;
        DestinationIsGoal = false;
        levelCompleted = Utils.FindIncludingInactive("CompletedCanvas");

	}

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate ()
    {
        //if (moving)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, movingDestination, speed * Time.deltaTime);
        //    moving = !transform.position.Equals(movingDestination);
        //}
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

                //speed = playerScript.speed;
                var movingDirection = playerScript.moveDirection;
                //movingDestination = transform.position + movingDirection;
                //lastPosition = transform.position;

                //moving = true;
                playerScript.StartPushing();
                playerScript.PushCount++;

                transform.parent = whoTouchedMe.transform;
                transform.position = whoTouchedMe.transform.position + movingDirection;
                break;
            case "Floor":
                SetIsGoal(false);
                break;
            case "Goal":
                SetIsGoal(true);
                break;
            case "Wall":
            case "Chest":
                //if (moving)
                //{
                //    movingDestination = lastPosition;
                    playerScript.MoveBack(true);
                //}
                break;
        }
    }

    private void SetIsGoal(bool value)
    {
        //Debug.Log("SetIsGoal --> " + value.ToString());

        if (DestinationIsGoal != value)
        {
            if (value)
            {
                gameManager.CurrentLevel.NumberOfChestsOnGoal++;
                if (gameManager.CurrentLevel.LevelCompleted)
                {
                    Debug.Log("Level Completed!!!");
                    gameManager.SaveToHighscores();
                    gameManager.SetNextLevel();
                    levelCompleted.GetComponent<Canvas>().enabled = true;
                }
            }
            else
            {
                gameManager.CurrentLevel.NumberOfChestsOnGoal--;
            }
        }

        //destinationWasGoal = DestinationIsGoal;
        DestinationIsGoal = value;
    }
}
