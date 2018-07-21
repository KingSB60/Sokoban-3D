using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public void RestartGame()
    {
        GameManager.Instance.GameSettings.LastEnabledLevel = 0;
        SceneManager.LoadScene("LevelScene");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void ShowSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
