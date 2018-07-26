using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedButtons : MonoBehaviour
{
    private GameObject mainMenuCanvas;
    private GameObject selectorCanvas;
    private GameObject completedCanvas;

    private void Awake()
    {
        mainMenuCanvas = Utils.FindIncludingInactive("MainMenuCanvas");
        selectorCanvas = Utils.FindIncludingInactive("LevelSelectorCanvas");
        completedCanvas = Utils.FindIncludingInactive("CompletedCanvas");
    }

    public void GoToLevelSelector()
    {
        completedCanvas.GetComponent<Canvas>().enabled = false;
        selectorCanvas.GetComponent<Canvas>().enabled = true;
    }
    public void GoToMainMenu()
    {
        completedCanvas.GetComponent<Canvas>().enabled = false;
        mainMenuCanvas.GetComponent<Canvas>().enabled = true;
    }
}
