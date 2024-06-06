using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
   [SerializeField] GameObject pauseMenu;
    bool isGamePaused = false;

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
            PauseGame();
            GameManager.Instance.OnNextWaveStartInvoke();
        }
    }

    void PauseGameInput(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    public void PauseGame()
    {



        //Pause
        if (isGamePaused == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isGamePaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            InputManager.Instance.DisableControllers();
        }
        //Unpause
        else
        {
            if (!DayNightCycleManager.Instance.IsDay)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isGamePaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            InputManager.Instance.EnableControllers();

        }


    }

    public bool IsGamePaused() { return isGamePaused; }
}
