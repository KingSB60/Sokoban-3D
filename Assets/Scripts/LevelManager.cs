using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Transform Walls;
    public Transform Floors;
    public Transform Chests;
    public TextMeshProUGUI levelText;
    public GameObject levelFinished;
    public GameObject levelPaused;

    public GameObject Player;
    public GameObject WallTemplate;
    public GameObject FloorTemplate;
    public GameObject GoalTemplate;
    public GameObject ChestTemplate;

    //private Level level;

    void Awake()
    {
        ClearLevel();
        //level = GameManager.Instance.Levels.CurrentLevel;
        BuildLevel();
    }

    private void ClearLevel()
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
    private void BuildLevel()
    {
        for (int z = 0; z < GameManager.Instance.Levels.CurrentLevel.Height; z++)
        {
            for (int x = 0; x < GameManager.Instance.Levels.CurrentLevel.Width; x++)
            {
                var element = GameManager.Instance.Levels.CurrentLevel[z][x];
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
    private void SetPlayer(int z, int x)
    {
        Player.transform.position= new Vector3((float)x, Player.transform.position.y, (float)(GameManager.Instance.Levels.CurrentLevel.Height - z));
    }
    private void CreateLevelObject(GameObject template, Transform parent, int z, int x)
    {
        Vector3 clonePosition = template.transform.position;
        clonePosition = new Vector3((float)x, template.transform.position.y, (float)(GameManager.Instance.Levels.CurrentLevel.Height - z));
        GameObject clone = Instantiate(template, clonePosition, Quaternion.identity, parent);
    }

    void Start()
    {
        SetLevelText();
        var canvs = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (var canvas in canvs)
        {
            if (canvas.gameObject.name == "CompletedCanvas")
            {
                levelFinished = canvas.gameObject;
                break;
            }
        }
    }

    private void SetLevelText()
    {
        levelText.text = String.Format("Level {0}", GameManager.Instance.Levels.CurrentLevel.Id);
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
            !GameManager.Instance.Levels.CurrentLevel.LevelCompleted)
        {
            if (GameManager.Instance.Levels.CurrentLevel.LevelPaused)
                ContinueGame();
            else
                PauseGame();
        }
    }

    public void GoNextLevel()
    {
        if (GameManager.Instance.Levels.LastLevelReached)
        {
            AllLevelsFinished();
        }
        else
        {
            GameManager.Instance.Levels.NextLevel();
            GameManager.Instance.Levels.CurrentLevel.MoveCount = 0;
            GameManager.Instance.Levels.CurrentLevel.PushCount = 0;
            GameManager.Instance.Levels.CurrentLevel.History = String.Empty;
            ClearLevel();
            BuildLevel();
            SetLevelText();

            levelFinished.SetActive(false);
            GameManager.Instance.Levels.CurrentLevel.StartTime = DateTime.Now;
        }
    }
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void AllLevelsFinished()
    {
        SceneManager.LoadScene("GameMenu");
    }
    public void GoToMainMenu()
    {
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
        GameManager.Instance.Levels.CurrentLevel.LevelPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Pause(bool value)
    {
        GameManager.Instance.Levels.CurrentLevel.LevelPaused = value;
        levelPaused.SetActive(value);
    }
}
