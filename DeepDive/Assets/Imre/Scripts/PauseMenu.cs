using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject miniMap;
    public GameObject HUD;
    public GameObject Finish;
    public bool isPaused = false;
    public AudioMixer audioMixer;

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    PlayerInput playerInput;

    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        HUD.SetActive(true);
        Finish.SetActive(false);
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && 
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerInput.actions["PauseGame"].triggered)
        {
            settingsMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (isPaused)
        {
            HUD.SetActive(false);
        }
        else
        {
            HUD.SetActive(true);
        }

        if (LapComplete.lapsDone == 3)
        {
            Finish.SetActive(true);
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void ReturnToMenu()
    {
        Finish.SetActive(false);
        HUD.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    void Pause()
    {
        pauseMenu.SetActive(true); 
        isPaused = true;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void EnableSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void DisableSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ToggleMap(bool isMapEnable)
    {
        miniMap.SetActive(isMapEnable);
    }
}
