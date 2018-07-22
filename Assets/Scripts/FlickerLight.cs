using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    public float maxTimerOn = 5f;
    public float maxTimerOff = 0.5f;
    public float minTimer = 0.05f;

    private GameObject lighter;
    private Material lighterMaterial;
    private float timer;

	// Use this for initialization
	void Start ()
    {
        lighter = transform.Find("Lighter").gameObject;
        lighterMaterial = lighter.GetComponent<Renderer>().material;
        StartCoroutine(FlickeringLight());
    }
	
    IEnumerator FlickeringLight()
    {
        SetEmission(true);
        timer = Random.Range(minTimer, maxTimerOn);
        yield return new WaitForSeconds(timer);
        SetEmission(false);
        timer = Random.Range(minTimer, maxTimerOff);
        yield return new WaitForSeconds(timer);
        StartCoroutine(FlickeringLight());
    }

    void SetEmission(bool val)
    {
        if (val)
            lighterMaterial.EnableKeyword("_EMISSION");
        else
            lighterMaterial.DisableKeyword("_EMISSION");
    }
}
