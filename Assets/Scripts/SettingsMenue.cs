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

    private GameManager gameManager;
    private List<string> levelFiles;

    private GameObject settingsCanvas;
    private GameObject mainMenu;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        mainMenu = Utils.FindIncludingInactive("MainMenuCanvas");
        settingsCanvas = Utils.FindIncludingInactive("GameSettingsCanvas");

        gameManager.GameSettings.PropertyChanged += GameSettings_PropertyChanged;

        pathInputField.text = gameManager.GameSettings.LevelDirectory;

        levelFilesDropdown.ClearOptions();
        levelFiles = LoadLevelFiles();
        levelFilesDropdown.AddOptions(levelFiles);
        int selected = levelFiles.IndexOf(gameManager.GameSettings.LevelFileName);
        if(selected==-1)
        {
            selected = 0;
            gameManager.GameSettings.LevelFileName = levelFiles[0];
        }
        levelFilesDropdown.value = selected;

        musicOnSwitch.isOn = gameManager.GameSettings.MusicOn;
        musicVolumeSlider.value = gameManager.GameSettings.MusicVolume;
        musicVolumeText.text = ((int)(gameManager.GameSettings.MusicVolume * 100f)).ToString();

        sfxOnSwitch.isOn = gameManager.GameSettings.SFXOn;
        sfxVolumeSlider.value = gameManager.GameSettings.SFXVolume;
        sfxVolumeText.text = ((int)(gameManager.GameSettings.SFXVolume * 100f)).ToString();

        masterVolumeSlider.value = gameManager.GameSettings.MasterVolume;
        masterVolumeText.text = ((int)(gameManager.GameSettings.MasterVolume * 100f)).ToString();
    }

    private void GameSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "MusicVolume":
                musicVolumeText.text = ((int)(gameManager.GameSettings.MusicVolume * 100f)).ToString();
                break;
            case "SFXVolume":
                sfxVolumeText.text = ((int)(gameManager.GameSettings.SFXVolume * 100f)).ToString();
                break;
            case "MasterVolume":
                masterVolumeText.text = ((int)(gameManager.GameSettings.MasterVolume * 100f)).ToString();
                break;
            case "MusicOn":
                musicVolumeSlider.interactable = gameManager.GameSettings.MusicOn;
                break;
            case "SFXOn":
                sfxVolumeSlider.interactable = gameManager.GameSettings.SFXOn;
                break;
            default:
                break;
        }
    }

    private List<string> LoadLevelFiles()
    {
        string path = gameManager.GameSettings.LevelDirectory;
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
        gameManager.GameSettings.LevelDirectory = pathInputField.text;
    }
    public void OnLevelFileChanged()
    {
        gameManager.GameSettings.LevelFileName = levelFiles[levelFilesDropdown.value];
    }
    public void OnMusicOnChanged()
    {
        gameManager.GameSettings.MusicOn = musicOnSwitch.isOn;
    }
    public void OnMusicVolumeChanged()
    {
        gameManager.GameSettings.MusicVolume = musicVolumeSlider.value;
    }
    public void OnSFXOnChanged()
    {
        gameManager.GameSettings.SFXOn = sfxOnSwitch.isOn;
    }
    public void OnSFXVolumeChanged()
    {
        gameManager.GameSettings.SFXVolume = sfxVolumeSlider.value;
    }
    public void OnMasterVolumeChanged()
    {
        gameManager.GameSettings.MasterVolume = masterVolumeSlider.value;
    }

    public void BackToMainMenu()
    {
        //SceneManager.LoadScene("GameMenu");
        settingsCanvas.GetComponent<Canvas>().enabled = false;
        mainMenu.GetComponent<Canvas>().enabled = true;
    }
}
