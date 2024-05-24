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
        InputManager.Instance.GetPlayerInputAction().AllTime.Pause.performed += PauseGame;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame(InputAction.CallbackContext context)
    {

        Debug.Log("Pauza");

        if (isGamePaused == false)
        {
            isGamePaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            InputManager.Instance.DisableControllers();
        }
        else
        {
            isGamePaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            InputManager.Instance.EnableControllers();

        }

    }


    //DO USUNIECIA PO NAPRAWWIENIU Z INPUT SYSTEMEM
    void PauseGame()
    {

        Debug.Log("Pauza");


        if (isGamePaused == false)
        {
            isGamePaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            InputManager.Instance.DisableControllers();
        }
        else
        {
            isGamePaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            InputManager.Instance.EnableControllers();

        }


    }

    public bool IsGamePaused() { return isGamePaused; }
}
