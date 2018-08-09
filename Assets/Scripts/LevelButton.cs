using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private GameObject minimapCanvas;
    private GameObject statusCanvas;
    private GameObject selectorCanvas;

    //public GameObject levelSelector;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        minimapCanvas = Utils.FindIncludingInactive("MinimapCanvas");
        statusCanvas = Utils.FindIncludingInactive("StatusCanvas");
        selectorCanvas = Utils.FindIncludingInactive("LevelSelectorCanvas");
    }

    public void InitButton(Level level, bool isEnabled)
    {
        Transform levelText = transform.Find("LevelPanel/LevelIdText");
        levelText.gameObject.GetComponent<TextMeshProUGUI>().text = level.Id;

        Transform previewGrid = transform.Find("LevelPanel/LevelPreviewBorder/LevelPreviewBackground/LevelPreviewGrid");
        previewGrid.gameObject.GetComponent<PreviewCreator>().CreatePreview(level, isEnabled);
    }

    public void LoadGameLevel()
    {
        Transform levelText = transform.Find("LevelPanel/LevelIdText");

        gameManager.CurrentLevelId = levelText.gameObject.GetComponent<TextMeshProUGUI>().text;
        LevelManager.Instance.ClearLevel();
        LevelManager.Instance.BuildLevel();
        LevelManager.Instance.SetLevelText();

        gameManager.CurrentLevel.MoveCount = 0;
        gameManager.CurrentLevel.PushCount = 0;
        gameManager.CurrentLevel.RotationCount = 0;
        gameManager.CurrentLevel.StartTime = DateTime.Now;
        //SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
        //Transform parent = transform.parent;
        //while (parent.gameObject.name != "LevelSelectorCanvas")
        //    parent = parent.parent;
        //parent.gameObject.SetActive(false);

        minimapCanvas.GetComponent<Canvas>().enabled = true;
        statusCanvas.GetComponent<Canvas>().enabled = true;
        selectorCanvas.GetComponent<Canvas>().enabled = false;
    }
}
