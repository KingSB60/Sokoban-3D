using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public class Settings : /*MonoBehaviour,*/ INotifyPropertyChanged
{
    private static string levelPath_Key = "LevelPath";

    private static string levelFile_Key = "LevelFile";
    private static string musicOn_Key = "MusicOn";
    private static string musicVol_Key = "MusicVolume";
    private static string sfxOn_Key = "SFXOn";
    private static string sfxVol_Key = "SFXVolume";
    private static string masterVol_Key = "MasterVolume";
    private static string lastResolved_Key = "LastEnabledLevel";

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public Settings()
    {
        if (!PlayerPrefs.HasKey(levelPath_Key))
            LevelDirectory = @".\GameData";
        if (!PlayerPrefs.HasKey(levelFile_Key))
            LevelFileName = string.Empty;
        if (!PlayerPrefs.HasKey(musicOn_Key))
            MusicOn = true;
        if (!PlayerPrefs.HasKey(musicVol_Key))
            MusicVolume = 0.5f;
        if (!PlayerPrefs.HasKey(sfxOn_Key))
            SFXOn = true;
        if (!PlayerPrefs.HasKey(sfxVol_Key))
            SFXVolume = 0.75f;
        if (!PlayerPrefs.HasKey(masterVol_Key))
            MasterVolume = 1.0f;
        if (!PlayerPrefs.HasKey(lastResolved_Key) || LastResolvedLevel < 1)
            LastResolvedLevel = 1;
    }

    public string LevelDirectory
    {
        get { return PlayerPrefs.GetString(levelPath_Key); }
        set
        {
            if (PlayerPrefs.GetString(levelPath_Key) != value)
            {
                PlayerPrefs.SetString(levelPath_Key, value);
                PlayerPrefs.Save();
                OnPropertyChanged("LevelDirectory");
            }
        }
    }
    public string LevelFileName
    {
        get { return PlayerPrefs.GetString(levelFile_Key); }
        set
        {
            if (PlayerPrefs.GetString(levelFile_Key) != value)
            {
                PlayerPrefs.SetString(levelFile_Key, value);
                PlayerPrefs.Save();
                OnPropertyChanged("LevelFileName");
            }
        }
    }
    public string LevelPath { get { return Path.Combine(LevelDirectory, LevelFileName + ".slc"); } }
    public bool MusicOn
    {
        get { return PlayerPrefs.GetInt(musicOn_Key) > 0; }
        set
        {
            int isSet = value ? 1 : 0;
            if (PlayerPrefs.GetInt(musicOn_Key) != isSet)
            {
                PlayerPrefs.SetInt(musicOn_Key, isSet);
                PlayerPrefs.Save();
                OnPropertyChanged("MusicOn");
            }
        }
    }
    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(musicVol_Key); }
        set
        {
            if (PlayerPrefs.GetFloat(musicVol_Key) != value)
            {
                PlayerPrefs.SetFloat(musicVol_Key, value);
                PlayerPrefs.Save();
                OnPropertyChanged("MusicVolume");
            }
        }
    }
    public bool SFXOn
    {
        get { return PlayerPrefs.GetInt(sfxOn_Key) > 0; }
        set
        {
            int isSet = value ? 1 : 0;
            if (PlayerPrefs.GetInt(sfxOn_Key) != isSet)
            {
                PlayerPrefs.SetInt(sfxOn_Key, isSet);
                PlayerPrefs.Save();
                OnPropertyChanged("SFXOn");
            }
        }
    }
    public float SFXVolume
    {
        get { return PlayerPrefs.GetFloat(sfxVol_Key); }
        set
        {
            if (PlayerPrefs.GetFloat(sfxVol_Key) != value)
            {
                PlayerPrefs.SetFloat(sfxVol_Key, value);
                PlayerPrefs.Save();
                OnPropertyChanged("SFXVolume");
            }
        }
    }
    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat(masterVol_Key); }
        set
        {
            if (PlayerPrefs.GetFloat(masterVol_Key) != value)
            {
                PlayerPrefs.SetFloat(masterVol_Key, value);
                PlayerPrefs.Save();
                OnPropertyChanged("MasterVolume");
            }
        }
    }
    public int LastResolvedLevel
    {
        get { return PlayerPrefs.GetInt(lastResolved_Key); }
        set
        {
            if (PlayerPrefs.GetInt(lastResolved_Key) != value)
            {
                if (PlayerPrefs.GetInt(lastResolved_Key) < value)
                {
                    PlayerPrefs.SetInt(lastResolved_Key, value);
                    PlayerPrefs.Save();
                    OnPropertyChanged("LastResolvedLevel");
                }
            }
        }
    }

    internal void ResetLastResolvedLevel()
    {
        PlayerPrefs.SetInt(lastResolved_Key, 0);
        PlayerPrefs.Save();
        OnPropertyChanged("LastResolvedLevel");
    }
}
