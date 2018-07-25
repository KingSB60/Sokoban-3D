using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Text FpsText;

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
    private GameObject mainMenu;

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
        for (int h = 0; h < gameManager.CurrentLevel.Height; h++)
        {
            for (int w = 0; w < gameManager.CurrentLevel.Width; w++)
            {
                var element = gameManager.CurrentLevel[h][w];
                switch (element)
                {
                    case LevelElement.Wall:
                        CreateLevelObject(WallTemplate, Walls, h, w);
                        break;
                    case LevelElement.Player:
                        CreateLevelObject(FloorTemplate, Floors, h, w);
                        SetPlayer(h, w);
                        break;
                    case LevelElement.PlayerOnGoal:
                        CreateLevelObject(GoalTemplate, Floors, h, w);
                        SetPlayer(h, w);
                        break;
                    case LevelElement.Box:
                        CreateLevelObject(FloorTemplate, Floors, h, w);
                        CreateLevelObject(ChestTemplate, Chests, h, w);
                        break;
                    case LevelElement.BoxOnGoal:
                        CreateLevelObject(GoalTemplate, Floors, h, w);
                        CreateLevelObject(ChestTemplate, Chests, h, w);
                        break;
                    case LevelElement.Goal:
                        CreateLevelObject(GoalTemplate, Floors, h, w);
                        break;
                    case LevelElement.Floor:
                        CreateLevelObject(FloorTemplate, Floors, h, w);
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
    private void CreateLevelObject(GameObject template, Transform parent, int h, int w)
    {
        Vector3 clonePosition = template.transform.position;
        clonePosition = new Vector3((float)w, template.transform.position.y, (float)(gameManager.CurrentLevel.Height - h));
        GameObject clone = Instantiate(template, clonePosition, Quaternion.identity, parent);
        if (clone.CompareTag("Wall"))
        {
            GameObject lampE = clone.transform.Find("NeonLighter_E").gameObject;
            GameObject lampS = clone.transform.Find("NeonLighter_S").gameObject;
            GameObject lampW = clone.transform.Find("NeonLighter_W").gameObject;
            GameObject lampN = clone.transform.Find("NeonLighter_N").gameObject;
            LevelElement[] neighbors = gameManager.CurrentLevel.GetNeighbors(h, w);
            lampE.SetActive(neighbors[0] != LevelElement.Empty &&
                            neighbors[0] != LevelElement.Wall &&
                            neighbors[0] != LevelElement.unknown);
            lampS.SetActive(neighbors[1] != LevelElement.Empty &&
                            neighbors[1] != LevelElement.Wall &&
                            neighbors[1] != LevelElement.unknown);
            lampW.SetActive(neighbors[2] != LevelElement.Empty &&
                            neighbors[2] != LevelElement.Wall &&
                            neighbors[2] != LevelElement.unknown);
            lampN.SetActive(neighbors[3] != LevelElement.Empty &&
                            neighbors[3] != LevelElement.Wall &&
                            neighbors[3] != LevelElement.unknown);
        }
    }

    void Start()
    {
        SetLevelText();
        //var canvs = Resources.FindObjectsOfTypeAll<Canvas>();
        levelFinished = Utils.FindIncludingInactive("CompletedCanvas");
        levelPaused = Utils.FindIncludingInactive("PausedCanvas");
        levelSelector = Utils.FindIncludingInactive("LevelSelectorCanvas");
        mainMenu = Utils.FindIncludingInactive("MainMEnuCanvas");

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

        float fps = 1f / Time.deltaTime;
        FpsText.text = Math.Ceiling(fps).ToString() + " FPS";
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
        levelFinished.GetComponent<Canvas>().enabled = false;
        mainMenu.GetComponent<Canvas>().enabled = true;
    }
    public void GoToMainMenu()
    {
        Pause(false);
        levelFinished.GetComponent<Canvas>().enabled = false;
        mainMenu.GetComponent<Canvas>().enabled = true;
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
