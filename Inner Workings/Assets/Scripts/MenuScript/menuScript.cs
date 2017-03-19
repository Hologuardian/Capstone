using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    //Variables for all menu items
    //Public Canvases
    public Canvas quitMenu;
    public Canvas startMenu;
    public Canvas optionsMenu;

    //Public Buttons
    public Button startText;
    public Button quitText;
    public Button optionText;
    public Button backText;
    public Button audioText;
    public Button videoText;

    //Game Objects for Options menus
    public GameObject audioStuff;
    public GameObject videoStuff;

    //Public Sliders
    public Slider sfxSlider;
    public Slider musicSlider;

    //Public AudioSources
    public AudioSource musicSource;
    public AudioSource sfxSource;

    //Aspect Ratio Variables
    public Toggle[] resToggles;
    public Toggle fullScreenToggle;
    public int[] screenWidths;
    int activeScreenResIndex;
    float sfxValue;
    float musicValue;

    void Start()
    {
        // Get all the menu components
        quitMenu = quitMenu.GetComponent<Canvas>();
        startMenu = startMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        quitText = quitText.GetComponent<Button>();
        optionText = optionText.GetComponent<Button>();
        audioText = audioText.GetComponent<Button>();
        videoText = videoText.GetComponent<Button>();

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

        //Disable the quit/options menu on start
        optionsMenu.enabled = false;
        quitMenu.enabled = false;
        audioStuff.SetActive(false);
        videoStuff.SetActive(false);
    }

    //Button to open options
    public void OptionPress()
    {
        //Enable the options menu and disable the start menu
        startMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    //Button to open quit menu
    public void ExitPress()
    {
        //Enable the quit menu and disable the start menu
        quitMenu.enabled = true;
        startMenu.enabled = false;
    }

    //Quit Menu Items
    public void NoPress()
    {
        //Re-enable the start menu and disable the quit menu
        quitMenu.enabled = false;
        startText.enabled = true;
        quitText.enabled = true;
        startMenu.enabled = true;
    }

    public void ExitGame()
    {
        //Quit the Game
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    //Play Button
    public void StartLevel(int num)
    {
        if(num < 0 || num >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Can't load scene num " + num + "SceneManager only has " + SceneManager.sceneCountInBuildSettings + "scenes in Build Settings");
            return;
        }

        LoadingSceneManager.LoadScene(num);

        DontDestroyOnLoad(optionsMenu);
    }

    //Options Menu Items
    public void BackPress()
    {
        optionsMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void AudioPress()
    {
        audioStuff.SetActive(true);
        videoStuff.SetActive(false);
    }

    public void VideoPress()
    {
        audioStuff.SetActive(false);
        videoStuff.SetActive(true);
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
}