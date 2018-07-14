using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public void RestartGame()
    {
        GameManager.Instance.GameSettings.LastEnabledLevel = 0;
        SceneManager.LoadScene("LevelSelector");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("LevelSelector");
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
