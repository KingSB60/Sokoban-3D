using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Transform Walls;
    public Transform Floors;
    public Transform Chests;
    public TextMeshProUGUI levelText;

    public GameObject Player;
    public GameObject WallTemplate;
    public GameObject FloorTemplate;
    public GameObject GoalTemplate;
    public GameObject ChestTemplate;

    private GameManager gameManager;
    private GameObject levelFinished;
    private GameObject levelPaused;
    private GameObject levelSelector;

    void Awake()
    {
        gameManager = GameManager.Instance;
        Instance = this;
        //ClearLevel();
        ////level = GameManager.Instance.Levels.CurrentLevel;
        //BuildLevel();
    }

    public void ClearLevel()
    {
        // remove all Wall elements
        for (int i = 0; i < Walls.childCount; i++)
            Destroy(Walls.GetChild(i).gameObject);

        // remove all Floor elements
        for (int i = 0; i < Floors.childCount; i++)
            Destroy(Floors.GetChild(i).gameObject);

        // remove all Chest elements
        for (int i = 0; i < Chests.childCount; i++)
            Destroy(Chests.GetChild(i).gameObject);
    }
    public void BuildLevel()
    {
        for (int z = 0; z < gameManager.CurrentLevel.Height; z++)
        {
            for (int x = 0; x < gameManager.CurrentLevel.Width; x++)
            {
                var element = gameManager.CurrentLevel[z][x];
                switch (element)
                {
                    case LevelElement.Wall:
                        CreateLevelObject(WallTemplate, Walls, z, x);
                        break;
                    case LevelElement.Player:
                        CreateLevelObject(FloorTemplate, Floors, z, x);
                        SetPlayer(z, x);
                        break;
                    case LevelElement.PlayerOnGoal:
                        CreateLevelObject(GoalTemplate, Floors, z, x);
                        SetPlayer(z, x);
                        break;
                    case LevelElement.Box:
                        CreateLevelObject(FloorTemplate, Floors, z, x);
                        CreateLevelObject(ChestTemplate, Chests, z, x);
                        break;
                    case LevelElement.BoxOnGoal:
                        CreateLevelObject(GoalTemplate, Floors, z, x);
                        CreateLevelObject(ChestTemplate, Chests, z, x);
                        break;
                    case LevelElement.Goal:
                        CreateLevelObject(GoalTemplate, Floors, z, x);
                        break;
                    case LevelElement.Floor:
                        CreateLevelObject(FloorTemplate, Floors, z, x);
                        break;
                }
            }
        }
    }
    public void SetLevelText()
    {
        levelText.text = gameManager.CurrentLevel.Id;
    }

    private void SetPlayer(int z, int x)
    {
        Player.transform.position= new Vector3((float)x, Player.transform.position.y, (float)(gameManager.CurrentLevel.Height - z));
    }
    private void CreateLevelObject(GameObject template, Transform parent, int z, int x)
    {
        Vector3 clonePosition = template.transform.position;
        clonePosition = new Vector3((float)x, template.transform.position.y, (float)(gameManager.CurrentLevel.Height - z));
        GameObject clone = Instantiate(template, clonePosition, Quaternion.identity, parent);
    }

    void Start()
    {
        SetLevelText();
        //var canvs = Resources.FindObjectsOfTypeAll<Canvas>();
        levelFinished = Utils.FindIncludingInactive("CompletedCanvas");
        levelPaused = Utils.FindIncludingInactive("PausedCanvas");
        levelSelector = Utils.FindIncludingInactive("LevelSelectorCanvas");

        //DontDestroyOnLoad(levelSelector);

        //foreach (var canvas in canvs)
        //{
        //    if (canvas.gameObject.name == "CompletedCanvas")
        //    {
        //        levelFinished = canvas.gameObject;
        //        break;
        //    }
        //}
    }

    private void Update()
    {
        bool p_pressed = Input.GetKeyDown(KeyCode.P);
        bool esc_pressed = Input.GetKeyDown(KeyCode.Escape);

        //if (p_pressed)
        //    Debug.Log("p_pressed!");
        //if (esc_pressed)
        //    Debug.Log("esc_pressed!");

        if ((p_pressed || esc_pressed) && 
            !gameManager.CurrentLevel.LevelCompleted)
        {
            if (gameManager.CurrentLevel.LevelPaused)
                ContinueGame();
            else
                PauseGame();
        }
    }

    public void GoNextLevel()
    {
        if (gameManager.LastLevelReached)
        {
            AllLevelsFinished();
        }
        else
        {
            gameManager.NextLevel();
            gameManager.CurrentLevel.MoveCount = 0;
            gameManager.CurrentLevel.PushCount = 0;
            gameManager.CurrentLevel.History = String.Empty;
            ClearLevel();
            BuildLevel();
            SetLevelText();

            levelFinished.GetComponent<Canvas>().enabled = false;
            gameManager.CurrentLevel.StartTime = DateTime.Now;
        }
    }
    public void GoToLevelSelect()
    {
        //SceneManager.LoadScene("LevelSelector");
        Pause(false);
        levelFinished.GetComponent<Canvas>().enabled = false;
        levelSelector.GetComponent<Canvas>().enabled = true;
    }
    public void AllLevelsFinished()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void GoToMainMenu()
    {
        Pause(false);
        SceneManager.LoadScene("GameMenu");
    }
    public void PauseGame()
    {
        Pause(true);
    }
    public void ContinueGame()
    {
        Pause(false);
    }
    public void RestartLevel()
    {
        gameManager.CurrentLevel.LevelPaused = false;

        int boxCount = 0;
        for (int z = 0; z < gameManager.CurrentLevel.Height; z++)
            for (int x = 0; x < gameManager.CurrentLevel.Width; x++)
            {
                switch (gameManager.CurrentLevel[z][x])
                {
                    case LevelElement.Player:
                    case LevelElement.PlayerOnGoal:
                        SetPlayer(z, x);
                        break;
                    case LevelElement.Box:
                        var chest = Chests.transform.GetChild(boxCount);
                        chest.position = new Vector3((float)(x), chest.position.y, (float)(gameManager.CurrentLevel.Height - z));
                        boxCount++;
                        break;
                    default:
                        break;
                }
            }

        Pause(false);
        gameManager.CurrentLevel.MoveCount = 0;
        gameManager.CurrentLevel.PushCount = 0;
        gameManager.CurrentLevel.StartTime = DateTime.Now;
    }

    private void Pause(bool value)
    {
        gameManager.CurrentLevel.LevelPaused = value;
        levelPaused.GetComponent<Canvas>().enabled = value;
    }
}
