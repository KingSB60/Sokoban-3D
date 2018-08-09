using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    internal enum RotationDirection
    {
        Left,
        Right
    }
    internal enum PlayerState
    {
        Idle,
        Rotating,
        Walking,
        Pushing
    }

    [Header("Movement")]
    public Vector3 moveDirection;
    public Vector3 oldPosition;
    public float speed;
    public float rotationSpeed;
    public Directions lookingTo;
    public Transform BoxContainer;
   //public Text movesText, pushsText, timeText;
    //public int chestsOnGoal;

    [Header("GUI")]
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI pushsText, rotationsText, timeText;
    public Text movingText, lookingText;

    [Header("Animation")]
    public GameObject animationAvatar;

    private bool moving, rotating;
    private Vector3 moveDestination;
    private float rotationAngle, destinationAngle;
    private float startAngle;
    private float rotationProgress;
    //private float axisX, axisZ;
    private GameManager gameManager;
    private bool rotateLeft, rotateRight;
    private bool move;
    private Animator animator;
    private PlayerState playerState;
    //private Transform animWrapper;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public int MoveCount
    {
        get { return gameManager.CurrentLevel.MoveCount; }
        set
        {
            gameManager.CurrentLevel.MoveCount = value;
            SetMoveText();
        }
    }
    public int PushCount
    {
        get { return gameManager.CurrentLevel.PushCount; }
        set
        {
            gameManager.CurrentLevel.PushCount = value;
            SetPushText();
        }
    }
    public int RotationCount
    {
        get { return gameManager.CurrentLevel.RotationCount; }
        set
        {
            gameManager.CurrentLevel.RotationCount = value;
            SetRotationText();
        }
    }

    // Use this for initialization
    void Start()
    {
        animator = animationAvatar.GetComponent<Animator>();
        //animWrapper = animationAvatar.transform.parent;

        moving = true;
        rotating = false;
        playerState = PlayerState.Idle;
        lookingTo = Directions.North;

        moveDestination = transform.position;
        oldPosition = transform.position;
        rotationAngle = 0f;
        moveDirection = Vector3.zero;
        MoveCount = 0;
        PushCount = 0;
        //startTime = DateTime.MinValue;
        gameManager.CurrentLevel.StartTime = DateTime.Now;
    }

    internal void StartPushing()
    {
        animator.SetTrigger("StartPushing");
        playerState = PlayerState.Pushing;
    }

    // Update is called once per frame
    void Update()
    {
        if ((gameManager.CurrentLevel.LevelCompleted ||
             gameManager.CurrentLevel.LevelPaused) &&
            !moving)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)||
            Input.GetKeyDown(KeyCode.A))
            rotateLeft = true;
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
            rotateRight = true;
        move = Input.GetKey(KeyCode.UpArrow) ||
               Input.GetKey(KeyCode.W);

        if (!rotating)
        {
            if(rotateLeft)
                StartRotation(RotationDirection.Left);

            if (rotateRight)
                StartRotation(RotationDirection.Right);
        }
        else
            Rotate();

        //float axisX = Input.GetAxis("Horizontal");
        //float axisZ = Input.GetAxis("Vertical");

        if (!moving && !rotating)
        {
            if (move)
                StartMoving();

            if (!moveDirection.Equals(Vector3.zero))
                MoveCount++;

            moveDestination = transform.position + moveDirection;
            oldPosition = transform.position;
        }
        else  if(!rotating)
            Moving();

        ClearAnimationsRootTransform();
        SetTimeText();

        movingText.text = "Moving: " + moving.ToString();
        lookingText.text = "LookAt: " + lookingTo.ToString();
    }

    private void ClearAnimationsRootTransform()
    {
        animationAvatar.transform.localPosition = Vector3.zero;
        animationAvatar.transform.localRotation = Quaternion.identity;
    }

    private void StartMoving()
    {
        switch (lookingTo)
        {
            case Directions.North:
                moveDirection = Vector3.forward;
                break;
            case Directions.East:
                moveDirection = Vector3.left;
                break;
            case Directions.South:
                moveDirection = Vector3.back;
                break;
            case Directions.West:
                moveDirection = Vector3.right;
                break;
        }
        //moveDirection = Vector3.forward;
        animator.SetBool("Walking", true);
        animator.SetTrigger("StartWalking");
        moving = true;
    }
    private void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveDestination, speed * Time.deltaTime);
        bool destinationReached = transform.position.Equals(moveDestination);
        if (destinationReached && move)
        {
            moveDestination = transform.position + moveDirection;
            oldPosition = transform.position;
            MoveCount++;
        }
        else
            moving = !destinationReached;

        if (!moving)
        {
            // moving destination reached
            moveDirection = Vector3.zero;
            // release parented Chest-object
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.CompareTag("Chest"))
                    child.parent = BoxContainer;
            }
            // cancel Animation
            animator.SetBool("Walking", false);
        }
    }

    private void StartRotation(RotationDirection direction)
    {
        rotationAngle = 90f;
        int look = (int)lookingTo;

        if (direction== RotationDirection.Left)
        {
            rotationAngle *= -1;
            look++;
            rotateLeft = false;
        }
        else
        {
            look--;
            rotateRight = false;
        }

        if (look > 3)
            look -= 4;
        if (look < 0)
            look += 4;
        lookingTo = (Directions)look;
        startAngle = transform.rotation.eulerAngles.y;
        destinationAngle = startAngle + rotationAngle;
        if (destinationAngle < 0f)
            destinationAngle += 360;
        rotationProgress = 0;

        animator.SetTrigger("Turn"+direction.ToString());
        rotating = true;
        RotationCount++;
    }
    private void Rotate()
    {
        if (rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, startAngle, 0), Quaternion.Euler(0, destinationAngle, 0), rotationProgress);
        }
        else
            rotating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            MoveBack(false);
        }
    }

    public void MoveBack(bool wasPush)
    {
        moveDirection = -moveDirection;
        moveDestination = oldPosition;
        MoveCount--;
        if (wasPush)
            PushCount--;
    }

    private void SetMoveText()
    {
        movesText.text = "Zuege: " + gameManager.CurrentLevel.MoveCount.ToString();
    }
    private void SetPushText()
    {
        pushsText.text = "Schuebe: " + gameManager.CurrentLevel.PushCount.ToString();
    }
    private void SetRotationText()
    {
        rotationsText.text = "Drehungen: " + gameManager.CurrentLevel.RotationCount.ToString();
    }
    private void SetTimeText()
    {
        string time_txt = "00:00,0";

        if(!gameManager.CurrentLevel.LevelCompleted)
        {
            gameManager.CurrentLevel.EndTime = DateTime.Now;
        }

        if(gameManager.CurrentLevel.StartTime!=DateTime.MinValue)
            time_txt = String.Format("{0:D2}:{1:D2}:{2:D2},{3}",
                                      gameManager.CurrentLevel.LevelTime.Hours,
                                      gameManager.CurrentLevel.LevelTime.Minutes,
                                      gameManager.CurrentLevel.LevelTime.Seconds,
                                      gameManager.CurrentLevel.LevelTime.Milliseconds / 100);

        timeText.text = "Zeit: " + time_txt;
    }
}
