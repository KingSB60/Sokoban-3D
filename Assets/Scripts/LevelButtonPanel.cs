using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonPanel : MonoBehaviour {

    public GameObject levelButtonTemplate;
    public Scrollbar selectorScrollbar;
    public ScrollRect selectorScrollRect;

    private GameManager gameManager;

    // Use this for initialization
    void Awake()
    {
        gameManager = GameManager.Instance;
        InitSelector();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void InitSelector()
    {
        foreach (Transform child in transform)
        {
            string msg = string.Format("try destroy {0} with name {1}", child.gameObject, child.gameObject.name);
            Debug.Log(msg);
            if (child.gameObject.CompareTag("LevelButton"))
                Destroy(child);
        }

        for (int idx = 0; idx < gameManager.LevelCount; idx++)
        {
            var level = gameManager.Levels[idx];
            bool isEnabled = gameManager.GetLevelIndex(level) <= gameManager.GameSettings.LastEnabledLevel;

            if (level.LevelButton == null)
            {
                if (level.LevelButton == null)
                {
                    level.LevelButton = Instantiate(levelButtonTemplate);
                    level.LevelButton.GetComponent<RectTransform>().localScale = Vector3.one;
                    level.LevelButton.GetComponent<LevelButton>().InitButton(level, isEnabled);
                    //DontDestroyOnLoad(level.LevelButton);
                }
            }
            else
            {
                level.LevelButton.transform.Find("LockPanel").gameObject.SetActive(!isEnabled);
                level.LevelButton.GetComponent<Button>().interactable = isEnabled;
            }
            //if (level.LevelButton.transform.parent != null)
            //    level.LevelButton.transform.SetParent(null, true);
            level.LevelButton.transform.SetParent(transform, false);
        }

        selectorScrollbar.value = (float)gameManager.GameSettings.LastEnabledLevel / (float)gameManager.LevelCount;
        selectorScrollRect.horizontalNormalizedPosition = (float)gameManager.GameSettings.LastEnabledLevel / (float)gameManager.LevelCount;
    }
}
