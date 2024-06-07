using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    bool isGamePaused = false;
    bool isInSettingsMenu = false;

    private static PauseManager _instance;
    public static PauseManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    void Start()
    {
        InputManager.Instance.GetPlayerInputAction().AllTime.Pause.performed += PauseGameInput;
    }

    private void Update()
    {
        //TODO: DELETE AFTER INPUT MANAGER FIX
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInSettingsMenu)
            {
                CloseSettings();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGameInput(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    public void PauseGame()
    {
        // Pause
        if (isGamePaused == false)
        {
            AudioManager.Instance.EnableMusicLowPassFilter(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isGamePaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            InputManager.Instance.DisableControllers();
        }
        // Unpause
        else
        {
            AudioManager.Instance.EnableMusicLowPassFilter(false);
            if (!DayNightCycleManager.Instance.IsDay)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isGamePaused = false;
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            isInSettingsMenu = false;       
            Time.timeScale = 1f;
            InputManager.Instance.EnableControllers();
        }
    }

    public bool IsGamePaused() { return isGamePaused; }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        isInSettingsMenu = true;
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        isInSettingsMenu = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAmbient("A_DayUI_Ambient");
        AudioManager.Instance.StopMusic("A_DayUI_Music");
        SceneManager.LoadScene(0);
        AudioManager.Instance.EnableMusicLowPassFilter(false);
    }
}

