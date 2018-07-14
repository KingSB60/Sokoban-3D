using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public class GameManager : INotifyPropertyChanged
{
    private static GameManager instance;

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    private GameManager()
    {
        GameSettings = new Settings();
        LoadLevels();
    }

    private void LoadLevels()
    {
        if(File.Exists(GameSettings.LevelPath))
            Levels = new SokobanLevels(GameSettings.LevelPath);
    }

    private void GameSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "LevelDirectory":
                LoadLevels();
                break;
            case "LevelFileName":
                LoadLevels();
                break;
            case "MusicOn":
                break;
            case "MusicVolume":
                break;
            case "SFXOn":
                break;
            case "SFXVolume":
                break;
            case "MasterVolume":
                break;
        }
        OnPropertyChanged("Settings." + e.PropertyName);
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    private SokobanLevels _Levels;
    public SokobanLevels Levels
    {
        get { return _Levels; }
        private set
        {
            _Levels = value;
            OnPropertyChanged("Levels");
        }
    }
    private Settings _GameSettings;
    public Settings GameSettings
    {
        get { return _GameSettings; }
        private set
        {
            _GameSettings = value;
            _GameSettings.PropertyChanged += GameSettings_PropertyChanged;
        }
    }

    public Level GetLevelById(string id)
    {
        return Levels.GetLevelById(id);
    }

    internal void SaveToHighscores()
    {
        //TODO: Save current levels re sults To Highscorelist!
    }
}
