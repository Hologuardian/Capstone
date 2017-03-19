using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Canvas optionsMenu;

    public GameObject pauseMenu;
    public GameObject optionsStuff;

    public Button optionsButton;
    public Button returnButton;
    public Button quitButton;
    public Button audioText;
    public Button videoText;

    public GameObject audioStuff;
    public GameObject videoStuff;

    public Slider sfxSlider;
    public Slider musicSlider;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Toggle[] resToggles;
    public Toggle fullScreenToggle;
    public int[] screenWidths;
    int activeScreenResIndex;
    float sfxValue;
    float musicValue;

    void Start ()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("Screen res index");
        bool isFullScreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

        for (int i = 0; i < resToggles.Length; i++)
        {
            resToggles[i].isOn = i == activeScreenResIndex;
        }

        fullScreenToggle.isOn = isFullScreen;

        musicValue = PlayerPrefs.GetFloat(Constants.musicValue);
        musicSlider.value = musicValue;

        sfxValue = PlayerPrefs.GetFloat(Constants.sfxValue);
        sfxSlider.value = sfxValue;

        optionsMenu.enabled = true;
        optionsStuff.SetActive(false);
        audioStuff.SetActive(false);
        videoStuff.SetActive(false);
    }

    public void SetScreenRes(int i)
    {
        if (resToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("Screen res index", activeScreenResIndex);
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        for (int i = 0; i < resToggles.Length; i++)
        {
            resToggles[i].interactable = !isFullScreen;
        }

        if (isFullScreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenRes(activeScreenResIndex);
        }

        PlayerPrefs.SetInt("Fullscreen", ((isFullScreen ? 1 : 0)));
        PlayerPrefs.Save();
    }

	public void ClosePressed()
    {

    }

    public void OptionsPressed()
    {
        optionsStuff.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void HubPressed()
    {
        SceneManager.LoadScene("HubWorld");
    }

    public void QuitPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void AudioPressed()
    {
        audioStuff.SetActive(true);
        videoStuff.SetActive(false);
    }

    public void VideoPressed()
    {
        audioStuff.SetActive(false);
        videoStuff.SetActive(true);
    }

    public void BackPressed()
    {
        optionsStuff.SetActive(false);
        audioStuff.SetActive(false);
        videoStuff.SetActive(false);

        pauseMenu.SetActive(true);
    }

    
    public void SetMusicAudio()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat(Constants.musicValue, musicSlider.value);
        PlayerPrefs.Save();
    }

    public void SetSFXAudio()
    {
        sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat(Constants.sfxValue, sfxSlider.value);
        PlayerPrefs.Save();
    }
}
