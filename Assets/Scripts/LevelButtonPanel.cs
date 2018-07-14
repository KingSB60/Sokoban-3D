using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonPanel : MonoBehaviour {

    public GameObject levelButtonTemplate;
    public Scrollbar selectorScrrollbar;

	// Use this for initialization
	void Awake ()
    {
        InitSelector();
	}

    void Start()
    {
        //selectorScrrollbar.value = 0;
    }

    // Update is called once per frame
    void Update ()
    {
		
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

        for (int idx = 0; idx < GameManager.Instance.Levels.Count; idx++)
        {
            var level = GameManager.Instance.Levels[idx];
            if (level.LevelButton == null)
            {
                bool isEnabled = GameManager.Instance.Levels.GetLevelIndex(level) <= GameManager.Instance.GameSettings.LastEnabledLevel;

                level.LevelButton = Instantiate(levelButtonTemplate);
                level.LevelButton.GetComponent<RectTransform>().localScale = Vector3.one;
                level.LevelButton.GetComponent<LevelButton>().InitButton(level, isEnabled);
                DontDestroyOnLoad(level.LevelButton);
            }
            level.LevelButton.transform.SetParent(transform, true);
        }
    }
}
