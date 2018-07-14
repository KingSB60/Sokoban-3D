using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
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

        GameManager.Instance.Levels.CurrentLevelId = levelText.gameObject.GetComponent<TextMeshProUGUI>().text;
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }
}
