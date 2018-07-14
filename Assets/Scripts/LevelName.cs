using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelName : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format("- {0} -", GameManager.Instance.Levels.Title);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
