using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [Header("Transformation")]
    public Transform target;
    public Vector3 offset;

    [Header("Zoom")]
    public int normalZoom = 1;
    public int fastZoom = 5;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);

        var scrolled = Input.GetAxis("Mouse ScrollWheel");
        var shift = Input.GetKey(KeyCode.LeftShift);
        int step = shift ? fastZoom : normalZoom;
        if (scrolled > 0)
            //GetComponent<Camera>().orthographicSize -= step;
            GetComponent<Camera>().fieldOfView -= step;
        else if (scrolled < 0)
            //GetComponent<Camera>().orthographicSize += step;
            GetComponent<Camera>().fieldOfView += step;
    }
}
