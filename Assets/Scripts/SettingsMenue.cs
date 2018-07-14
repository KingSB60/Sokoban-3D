using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class SettingsMenue : MonoBehaviour
{
    public TMP_InputField pathInputField;
    public TMP_Dropdown levelFilesDropdown;
    public Switch musicOnSwitch;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeText;
    public Switch sfxOnSwitch;
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI sfxVolumeText;
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeText;

    private List<string> levelFiles;

    private void Awake()
    {
        GameManager.Instance.GameSettings.PropertyChanged += GameSettings_PropertyChanged;

        pathInputField.text = GameManager.Instance.GameSettings.LevelDirectory;

        levelFilesDropdown.ClearOptions();
        levelFiles = LoadLevelFiles();
        levelFilesDropdown.AddOptions(levelFiles);
        int selected = levelFiles.IndexOf(GameManager.Instance.GameSettings.LevelFileName);
        if(selected==-1)
        {
            selected = 0;
            GameManager.Instance.GameSettings.LevelFileName = levelFiles[0];
        }
        levelFilesDropdown.value = selected;

        musicOnSwitch.isOn = GameManager.Instance.GameSettings.MusicOn;
        musicVolumeSlider.value = GameManager.Instance.GameSettings.MusicVolume;
        musicVolumeText.text = ((int)(GameManager.Instance.GameSettings.MusicVolume * 100f)).ToString();

        sfxOnSwitch.isOn = GameManager.Instance.GameSettings.SFXOn;
        sfxVolumeSlider.value = GameManager.Instance.GameSettings.SFXVolume;
        sfxVolumeText.text = ((int)(GameManager.Instance.GameSettings.SFXVolume * 100f)).ToString();

        masterVolumeSlider.value = GameManager.Instance.GameSettings.MasterVolume;
        masterVolumeText.text = ((int)(GameManager.Instance.GameSettings.MasterVolume * 100f)).ToString();
    }

    private void GameSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "MusicVolume":
                musicVolumeText.text = ((int)(GameManager.Instance.GameSettings.MusicVolume * 100f)).ToString();
                break;
            case "SFXVolume":
                sfxVolumeText.text = ((int)(GameManager.Instance.GameSettings.SFXVolume * 100f)).ToString();
                break;
            case "MasterVolume":
                masterVolumeText.text = ((int)(GameManager.Instance.GameSettings.MasterVolume * 100f)).ToString();
                break;
            case "MusicOn":
                musicVolumeSlider.interactable = GameManager.Instance.GameSettings.MusicOn;
                break;
            case "SFXOn":
                sfxVolumeSlider.interactable = GameManager.Instance.GameSettings.SFXOn;
                break;
            default:
                break;
        }
    }

    private List<string> LoadLevelFiles()
    {
        string path = GameManager.Instance.GameSettings.LevelDirectory;
        List<string> result = new List<string>();
        if (Directory.Exists(path))
        {
            foreach (var file in Directory.GetFiles(path, "*.slc", SearchOption.TopDirectoryOnly))
                result.Add(Path.GetFileNameWithoutExtension(file));
        }
        return result;
    }

    public void OnLevelDirectoryChanged()
    {
        GameManager.Instance.GameSettings.LevelDirectory = pathInputField.text;
    }
    public void OnLevelFileChanged()
    {
        GameManager.Instance.GameSettings.LevelFileName = levelFiles[levelFilesDropdown.value];
    }
    public void OnMusicOnChanged()
    {
        GameManager.Instance.GameSettings.MusicOn = musicOnSwitch.isOn;
    }
    public void OnMusicVolumeChanged()
    {
        GameManager.Instance.GameSettings.MusicVolume = musicVolumeSlider.value;
    }
    public void OnSFXOnChanged()
    {
        GameManager.Instance.GameSettings.SFXOn = sfxOnSwitch.isOn;
    }
    public void OnSFXVolumeChanged()
    {
        GameManager.Instance.GameSettings.SFXVolume = sfxVolumeSlider.value;
    }
    public void OnMasterVolumeChanged()
    {
        GameManager.Instance.GameSettings.MasterVolume = masterVolumeSlider.value;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
