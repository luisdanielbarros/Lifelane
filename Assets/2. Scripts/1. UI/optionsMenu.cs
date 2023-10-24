using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class optionsMenu : MonoBehaviour
{
    //Audio
    [SerializeField]
    private AudioMixer audioMixer;
    //Screen Resolution
    [SerializeField]
    private Resolution[] screenResolutions;
    [SerializeField]
    private TMP_Dropdown screenResolutionDropdown;
    void Start()
    {
        //Screen Resolution
        screenResolutions = Screen.resolutions;
        screenResolutionDropdown.ClearOptions();
        List<string> dropdownOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            string Option = screenResolutions[i].width + " x " + screenResolutions[i].height;
            if (screenResolutions[i].width == Screen.currentResolution.width &&
                screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            dropdownOptions.Add(Option);
        }
        screenResolutionDropdown.AddOptions(dropdownOptions);
        screenResolutionDropdown.value = currentResolutionIndex;
        screenResolutionDropdown.RefreshShownValue();
    }
    //Audio
    //Audio -> Master Volume
    public void setMasterVolume(float masterVolume)
    {
        masterVolume = (masterVolume * 0.8f) - 80f;
        audioMixer.SetFloat("masterVolume", masterVolume);
    }
    //Audio -> Music Volume
    public void setMusicVolume(float musicVolume)
    {
        musicVolume = (musicVolume * 0.8f) - 80f;
        audioMixer.SetFloat("musicVolume", musicVolume);
    }
    //Audio -> Environment Volume
    public void setEnvironmentVolume(float environmentVolume)
    {
        environmentVolume = (environmentVolume * 0.8f) - 80f;
        audioMixer.SetFloat("environmentVolume", environmentVolume);
    }
    //Audio -> UI SFX Volume
    public void setUiSfxVolume(float UiSfxVolume)
    {
        UiSfxVolume = (UiSfxVolume * 0.8f) - 80f;
        audioMixer.SetFloat("UiSfxVolume", UiSfxVolume);
    }
    //Graphics Quality
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    //Screen Size
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    //Screen Resolution
    public void setScreenResolution(int resolutionIndex)
    {
        Resolution newReslution = screenResolutions[resolutionIndex];
        Screen.SetResolution(newReslution.width, newReslution.height, Screen.fullScreen);
    }
    //Back
    public void Back()
    {
        gameStates previousGameState = gameState.Instance.previousState;
        if (previousGameState == gameStates.MainMenu || previousGameState == gameStates.overworldMenu)
        {
            gameState.Instance.setGameState(previousGameState);
        }
    }
}
