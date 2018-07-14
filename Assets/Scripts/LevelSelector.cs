using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Scrollbar selectorScrollbar;
    public ScrollRect selectorScrollRect;

    // Use this for initialization
    void Start()
    {
        //selectorScrollbar.value = 0;
        selectorScrollRect.horizontalNormalizedPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
