using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class SokobanLevels:IEnumerable<Level>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public LevelCollection Collection { get; private set; }
    public int Count { get { return Collection.Count; } }
    public string CurrentLevelId { get; set; }
    public Level CurrentLevel { get { return GetLevelById(CurrentLevelId); } }
    public string NextLevelId { get { return Collection.GetNextLevelId(CurrentLevelId); } }

    public bool LastLevelReached { get { return CurrentLevelId == Collection[Count - 1].Id; } }

    public int NextLevelIdx { get { return GetLevelIndex(NextLevelId); } }

    public SokobanLevels(string path)
    {
        LoadSokobanLevels(path);
    }

    private void LoadSokobanLevels(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement;
        foreach (XmlNode node in root.ChildNodes)
        {
            switch (node.Name)
            {
                case "Title":
                    Title = node.InnerText;
                    break;
                case "Description":
                    Description = node.InnerText;
                    break;
                case "LevelCollection":
                    Collection = new LevelCollection(node);
                    break;
                default:
                    break;
            }
        }

        if (Collection.Count > 0)
            CurrentLevelId = Collection[0].Id;
    }

    internal int GetLevelIndex(string levelId)
    {
        for (int idx = 0; idx < Count; idx++)
        {
            if (Collection[idx].Id==levelId)
                return idx;
        }
        return -1;
    }
    internal int GetLevelIndex(Level level)
    {
        for (int idx = 0; idx < Count; idx++)
        {
            if (Collection[idx].Equals(level))
                return idx;
        }
        return -1;
    }

    public Level this[int i]
    {
        get { return Collection[i]; }
    }

    internal Level GetLevelById(string id)
    {
        return Collection.GetLevelById(id);
    }
    public void NextLevel()
    {
        if (LastLevelReached)
            return;

        CurrentLevelId = NextLevelId;
    }

    #region IEnumerable<Level> member
    public IEnumerator<Level> GetEnumerator()
    {
        return Collection.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Collection.GetEnumerator();
    }
    #endregion
}

public class LevelCollection : IEnumerable<Level>
{
    public string Copyright { get; private set; }
    public int MaxWidth { get; private set; }
    public int MaxHeight { get; private set; }
    public int Count { get { return LevelList.Count; } }
    public List<Level> LevelList { get; private set; }

    public LevelCollection()
    {
        LevelList = new List<Level>();
    }
    public LevelCollection(XmlNode node):this()
    {
        foreach (XmlAttribute attr in node.Attributes)
        {
            switch (attr.Name)
            {
                case "Copyright":
                    Copyright = attr.Value;
                    break;
                case "MaxWidth":
                    MaxWidth = int.Parse(attr.Value);
                    break;
                case "MaxHeight":
                    MaxHeight = int.Parse(attr.Value);
                    break;
                default:
                    break;
            }
        }
        foreach (XmlNode levelNode in node.ChildNodes)
        {
            if (levelNode.Name == "Level")
                LevelList.Add(new Level(levelNode));
        }
    }

    internal Level this[int i] { get { return LevelList[i]; } }
    internal Level GetLevelById(string id)
    {
        foreach (Level level in LevelList)
        {
            if (level.Id == id)
                return level;
        }
        return null;
    }
    internal string GetNextLevelId(string currentId)
    {
        Level current = GetLevelById(currentId);
        int idx = LevelList.IndexOf(current);
        return LevelList[idx + 1].Id;
    }

    #region IEnumerable<Level> member
    public IEnumerator<Level> GetEnumerator()
    {
        return LevelList.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return LevelList.GetEnumerator();
    }
    #endregion
}

public class Level : IEnumerable<LevelLine>
{
    public string Id { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<LevelLine> Lines { get; private set; }

    public int NumberOfChests { get; set; }
    public int NumberOfChestsOnGoal { get; set; }
    public bool LevelCompleted { get { return NumberOfChests == NumberOfChestsOnGoal; } }
    private bool _LevelPaused;
    public bool LevelPaused
    {
        get { return _LevelPaused; }
        set
        {
            _LevelPaused = value;
            if (value)
                _PauseStart = DateTime.Now;
            else
                StartTime += DateTime.Now - _PauseStart;
        }
    }

    private DateTime _PauseStart;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan LevelTime { get { return EndTime - StartTime; } }

    public int MoveCount { get; set; }
    public int PushCount { get; set; }
    public String History { get; set; }

    public GameObject LevelButton { get; set; }

    public LevelLine this[int i]
    {
        get { return Lines[i]; }
    }

    public Level(XmlNode node)
    {
        foreach (XmlAttribute attr in node.Attributes)
        {
            switch (attr.Name)
            {
                case "Id":
                    Id = attr.Value;
                    break;
                case "Width":
                    Width = int.Parse(attr.Value);
                    break;
                case "Height":
                    Height = int.Parse(attr.Value);
                    break;
                default:
                    break;
            }
        }

        Lines = new List<LevelLine>();
        foreach (XmlNode lineNode in node.ChildNodes)
        {
            if (lineNode.Name == "L")
                Lines.Add(new LevelLine(lineNode.InnerText,Width));
        }

        ClearOuterFloor();
        CountChests();

        StartTime = DateTime.MinValue;
        EndTime = DateTime.MaxValue;
        MoveCount = 0;
        PushCount = 0;
        History = String.Empty;
    }

    private void CountChests()
    {
        NumberOfChests = 0;
        NumberOfChestsOnGoal = 0;
        foreach (var line in Lines)
            foreach (var item in line)
            {
                switch (item)
                {
                    case LevelElement.Box:
                        NumberOfChests++;
                        break;
                    case LevelElement.BoxOnGoal:
                        NumberOfChests++;
                        NumberOfChestsOnGoal++;
                        break;
                    default:
                        break;
                }
            }
    }
    private void ClearOuterFloor()
    {
        int i, j;

        #region obere Kante
        i = 0;
        for (j = 0; j < Width; j++)
                ClearElement(i, j);
        #endregion
        #region untere Kante
        i = Height - 1;
        for (j = 0; j < Width; j++)
            ClearElement(i, j);
        #endregion
        #region linke Kante
        j = 0;
        for (i = 0; i < Height; i++)
            ClearElement(i, j);
        #endregion
        #region obere Kante
        j = Width - 1;
        for (i = 0; i < Height; i++)
            ClearElement(i, j);
        #endregion
    }

    private void ClearElement(int i, int j)
    {
        if ( i < 0 || i >= Height ||
             j < 0 || j >= Width  ||
             this[i][j] != LevelElement.Floor)
            return;

        this[i][j] = LevelElement.Empty;
        ClearElement(i - 1, j - 1);
        ClearElement(i - 1, j);
        ClearElement(i - 1, j + 1);
        ClearElement(i, j - 1);
        ClearElement(i, j + 1);
        ClearElement(i + 1, j - 1);
        ClearElement(i + 1, j);
        ClearElement(i + 1, j + 1);
    }

    #region IEnumerable<LevelLine> member
    public IEnumerator<LevelLine> GetEnumerator()
    {
        return Lines.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Lines.GetEnumerator();
    }

    internal LevelElement[] GetNeighbors(int h, int w)
    {
        List<LevelElement> result = new List<LevelElement>();

        // East
        result.Add(w >= Width - 1 ? LevelElement.unknown : this[h][w + 1]);
        // South
        result.Add(h >= Height - 1 ? LevelElement.unknown : this[h + 1][w]);
        // West
        result.Add(w <= 0 ? LevelElement.unknown : this[h][w - 1]);
        // North
        result.Add(h <= 0 ? LevelElement.unknown : this[h - 1][w]);

        return result.ToArray();
    }
    #endregion
}

public enum LevelElement
{
    Wall         = (byte)'#',
    Player       = (byte)'@',
    PlayerOnGoal = (byte)'+',
    Box          = (byte)'$',
    BoxOnGoal    = (byte)'*',
    Goal         = (byte)'.',
    Floor        = (byte)' ',
    Empty        = 0,
    unknown      = -1
}

public class LevelLine : IEnumerable<LevelElement>
{
    public List<LevelElement> Elements { get; private set; }

    public LevelLine()
    {
        Elements = new List<LevelElement>();
    }
    public LevelLine(string line, int width):this()
    {
        for(int i=0; i<width;i++)
        {
            Elements.Add(i < line.Length ? (LevelElement)(line[i]) : LevelElement.Floor);
        }
    }

    public LevelElement this[int i]
    {
        get { return Elements[i]; }
        set { Elements[i] = value; }
    }

    #region IEnumerable<LevelElement> member
    public IEnumerator<LevelElement> GetEnumerator()
    {
        return ((IEnumerable<LevelElement>)Elements).GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<LevelElement>)Elements).GetEnumerator();
    }
    #endregion
}