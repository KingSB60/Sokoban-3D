using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    private GameManager gameManager;

    private GameObject selectorCanvas;
    private GameObject mainMenuCanvas;
    private GameObject settingsCanvas;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        selectorCanvas = Utils.FindIncludingInactive("LevelSelectorCanvas");
        mainMenuCanvas = Utils.FindIncludingInactive("MainMenuCanvas");
        settingsCanvas = Utils.FindIncludingInactive("GameSettingsCanvas");
    }

    public void RestartGame()
    {
        gameManager.GameSettings.ResetLastResolvedLevel();
        mainMenuCanvas.GetComponent<Canvas>().enabled = false;
        selectorCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void ContinueGame()
    {
        mainMenuCanvas.GetComponent<Canvas>().enabled = false;
        selectorCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void ShowSettings()
    {
        mainMenuCanvas.GetComponent<Canvas>().enabled = false;
        settingsCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
