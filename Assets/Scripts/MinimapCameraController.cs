using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    public int normalZoom = 1;
    public int fastZoom = 5;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        var scrolled = Input.GetAxis("Mouse ScrollWheel");
        var shift = Input.GetKey(KeyCode.LeftShift);
        int step = shift ? fastZoom : normalZoom;
        if (scrolled > 0)
            GetComponent<Camera>().orthographicSize += step;
        else if (scrolled < 0)
            GetComponent<Camera>().orthographicSize -= step;
    }
}
