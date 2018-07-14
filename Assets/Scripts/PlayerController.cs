using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Vector3 moveDirection, oldPosition;
    public float speed;
    //public Text movesText, pushsText, timeText;
    public TextMeshProUGUI movesText, pushsText, timeText;
    //public int chestsOnGoal;

    private bool moving;
    private Vector3 moveDestination;
    private float axisX, axisZ;
    //private DateTime startTime;

    //private int moveCount;
    //private int pushCount;

    public int MoveCount
    {
        get { return GameManager.Instance.Levels.CurrentLevel.MoveCount; }
        set
        {
            GameManager.Instance.Levels.CurrentLevel.MoveCount = value;
            SetMoveText();
        }
    }
    public int PushCount
    {
        get { return GameManager.Instance.Levels.CurrentLevel.PushCount; }
        set
        {
            GameManager.Instance.Levels.CurrentLevel.PushCount = value;
            SetPushText();
        }
    }

    // Use this for initialization
    void Start()
    {
        moving = true;

        moveDestination = transform.position;
        oldPosition = transform.position;
        moveDirection = Vector3.zero;
        MoveCount = 0;
        PushCount = 0;
        //startTime = DateTime.MinValue;
        GameManager.Instance.Levels.CurrentLevel.StartTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.Instance.Levels.CurrentLevel.LevelCompleted ||
             GameManager.Instance.Levels.CurrentLevel.LevelPaused) &&
            !moving)
            return;

        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");

        if (!moving)
        {
            if (axisX != 0f)
            {
                if (axisX > 0f)
                {
                    moveDirection = Vector3.right;
                    moving = true;
                }
                else if (axisX < 0f)
                {
                    moveDirection = Vector3.left;
                    moving = true;
                }
            }

            if (axisZ != 0f)
            {
                if (axisZ > 0f)
                {
                    moveDirection = Vector3.forward;
                    moving = true;
                }
                else if (axisZ < 0f)
                {
                    moveDirection = Vector3.back;
                    moving = true;
                }
            }

            if (!moveDirection.Equals(Vector3.zero))
            {
                MoveCount++;
                //if (startTime == DateTime.MinValue)
                //    startTime = DateTime.Now;
            }

            moveDestination = transform.position + moveDirection;
            oldPosition = transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, moveDestination, speed * Time.deltaTime);
            moving = !transform.position.Equals(moveDestination);
            if (!moving)
                moveDirection = Vector3.zero;
        }

        SetTimeText();
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
        movesText.text = "Zuege: " + GameManager.Instance.Levels.CurrentLevel.MoveCount.ToString();
    }
    private void SetPushText()
    {
        pushsText.text = "Schuebe: " + GameManager.Instance.Levels.CurrentLevel.PushCount.ToString();
    }
    private void SetTimeText()
    {
        string time_txt = "00:00,0";

        if(!GameManager.Instance.Levels.CurrentLevel.LevelCompleted)
        {
            GameManager.Instance.Levels.CurrentLevel.EndTime = DateTime.Now;
        }

        if(GameManager.Instance.Levels.CurrentLevel.StartTime!=DateTime.MinValue)
            time_txt = String.Format("{0:D2}:{1:D2},{2}",
                                      GameManager.Instance.Levels.CurrentLevel.LevelTime.Minutes,
                                      GameManager.Instance.Levels.CurrentLevel.LevelTime.Seconds,
                                      GameManager.Instance.Levels.CurrentLevel.LevelTime.Milliseconds / 100);

        timeText.text = "Zeit: " + time_txt;
    }
}
