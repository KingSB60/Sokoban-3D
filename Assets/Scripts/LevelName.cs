using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelName : MonoBehaviour {

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    // Use this for initialization
    void Start ()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format("- {0} -", gameManager.LevelsTitle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
