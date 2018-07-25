using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewCreator : MonoBehaviour
{
    public Texture2D Character;
    public Texture2D CharacterOnGoal;
    public Texture2D Chest;
    public Texture2D ChestOnGoal;
    public Texture2D Wall;
    public Texture2D Floor;
    public Texture2D Goal;

    //public GameObject Character;
    //public GameObject CharacterOnGoal;
    //public GameObject Chest;
    //public GameObject ChestOnGoal;
    //public GameObject Wall;
    //public GameObject Floor;
    //public GameObject Goal;
    //public GameObject Empty;

    public void Awake()
    {
        //Empty = new GameObject();
        //Empty.AddComponent<RectTransform>();
    }

    public void CreatePreview(Level level, bool isEnabled)
    {
        //return;

        RectTransform rectTransform = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        float panelMaxWidth = rectTransform.rect.width - 20;
        float panelMaxHeight = rectTransform.rect.height - 20;
        float objectWidth = panelMaxWidth / level.Width;
        float objectHeight = panelMaxHeight / level.Height;

        float elementSize = Mathf.Min(objectWidth, objectHeight);
        float panelWidth = elementSize * level.Width;
        float panelHeight = elementSize * level.Height;

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);

        Texture2D texture = new Texture2D(level.Width * 64, level.Height * 64);

        Texture2D original = null;
        for (int h = 0; h < level.Height; h++)
        {
            var line = level[h];
            for (int w = 0; w < level.Width; w++)
            {
                var item = line[w];
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
                    default:
                        original = null;
                        break;
                }
                InsertItemSprite(texture, original, h, w);
            }
        }
        texture.Apply();
        GetComponent<RawImage>().texture = texture;

        // TODO: Textur als .PNG speichern für spätere Nutzung
        string path = Application.persistentDataPath;

        #region alte Version
        //GameObject original = null;
        //for (int i = 0; i < level.Height; i++)
        //{
        //    var line = level[i];
        //    for (int j = 0; j < level.Width; j++)
        //    {
        //        var item = line[j];
        //        switch (item)
        //        {
        //            case LevelElement.Wall:
        //                original = Wall;
        //                break;
        //            case LevelElement.Player:
        //                original = Character;
        //                break;
        //            case LevelElement.PlayerOnGoal:
        //                original = CharacterOnGoal;
        //                break;
        //            case LevelElement.Box:
        //                original = Chest;
        //                break;
        //            case LevelElement.BoxOnGoal:
        //                original = ChestOnGoal;
        //                break;
        //            case LevelElement.Goal:
        //                original = Goal;
        //                break;
        //            case LevelElement.Floor:
        //                original = Floor;
        //                break;
        //            case LevelElement.Empty:
        //                original = Empty;
        //                break;
        //        }

        //        float posX = elementSize * j + elementSize / 2 - panelWidth / 2;
        //        float posY = -(elementSize * i + elementSize / 2) + panelHeight / 2;
        //        Vector3 elementPosition = new Vector3(posX, posY, 0);
        //        GameObject element = Instantiate(original, elementPosition, Quaternion.identity);

        //        element.GetComponent<RectTransform>().sizeDelta = Vector2.one * 50;
        //        ScaleElement(element, objectWidth, objectHeight);
        //        element.transform.SetParent(transform, false);
        //    }
        //}
        #endregion

        // in parents nach LevelButton-Objekt suchen...
        GameObject go = gameObject;
        while (!go.CompareTag("LevelButton"))
            go = go.transform.parent.gameObject;

        go.transform.Find("LockPanel").gameObject.SetActive(!isEnabled);
        go.GetComponent<Button>().interactable = isEnabled;
    }

    private void InsertItemSprite(Texture2D texture, Texture2D sprite, int h, int w)
    {
        if (sprite != null)
        {
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                {
                    int pixelX = w * 64 + x;
                    int pixelY = texture.height - ((h+1) * 64) + y;
                    texture.SetPixel(pixelX, pixelY, sprite.GetPixel(x, y));
                }
        }
    }

    private void ScaleElement(GameObject element, float gridWidth, float gridHeight)
    {
        float elementWidth = element.GetComponent<RectTransform>().rect.width;
        float elementHeight = element.GetComponent<RectTransform>().rect.height;
        float hScale = gridWidth / elementWidth;
        float vScale = gridHeight / elementHeight;

        float scale = Mathf.Min(vScale, hScale);
        element.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
    }
}
