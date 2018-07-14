using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewCreator : MonoBehaviour
{
    public GameObject Character;
    public GameObject CharacterOnGoal;
    public GameObject Chest;
    public GameObject ChestOnGoal;
    public GameObject Wall;
    public GameObject Floor;
    public GameObject Goal;
    public GameObject Empty;

    public void Awake()
    {
        //Empty = new GameObject();
        //Empty.AddComponent<RectTransform>();
    }

    public void CreatePreview(Level level, bool isEnabled)
    {
        RectTransform rectTransform = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        float totalWidth = rectTransform.rect.width - 20;
        float totalHeight = rectTransform.rect.height - 20;
        float objectWidth = totalWidth / level.Width;
        float objectHeight = totalHeight / level.Height;

        var glg = GetComponent<GridLayoutGroup>();
        glg.constraintCount = level.Width;
        glg.cellSize = Vector2.one * Math.Min(objectWidth, objectHeight);

        GameObject original = null;
        foreach (var line in level.Lines)
            foreach (var item in line)
            {
                switch (item)
                {
                    case LevelElement.Wall:
                        original = Wall;
                        break;
                    case LevelElement.Player:
                        original = Character;
                        break;
                    case LevelElement.PlayerOnGoal:
                        original = CharacterOnGoal;
                        break;
                    case LevelElement.Box:
                        original = Chest;
                        break;
                    case LevelElement.BoxOnGoal:
                        original = ChestOnGoal;
                        break;
                    case LevelElement.Goal:
                        original = Goal;
                        break;
                    case LevelElement.Floor:
                        original = Floor;
                        break;
                    case LevelElement.Empty:
                        original = Empty;
                        break;
                }
                GameObject element = Instantiate(original, Vector3.zero, Quaternion.identity);
                element.GetComponent<RectTransform>().sizeDelta = Vector2.one * 50;

                ScaleElement(element, objectWidth, objectHeight);
                element.transform.SetParent(transform, true);
            }

        GameObject go = gameObject;
        while (!go.CompareTag("LevelButton"))
            go = go.transform.parent.gameObject;

        go.transform.Find("LockPanel").gameObject.SetActive(!isEnabled);
        go.GetComponent<Button>().interactable = isEnabled;
    }

    private void ScaleElement(GameObject element, float gridWidth, float gridHeight)
    {
        float elementWidth = element.GetComponent<RectTransform>().rect.width;
        float elementHeight = element.GetComponent<RectTransform>().rect.height;
        float hScale = gridWidth / elementWidth;
        float vScale = gridHeight / elementHeight;

        float scale = Math.Min(vScale, hScale);
        element.GetComponent<RectTransform>().localScale = new Vector3(scale, scale);
    }
}
