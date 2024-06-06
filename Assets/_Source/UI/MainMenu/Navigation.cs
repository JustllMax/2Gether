using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject canvasMainMenu;
    public GameObject canvasSettings;
    public GameObject canvasCredits;

    void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasCredits.activeSelf || canvasSettings.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        canvasMainMenu.SetActive(false);
        canvasSettings.SetActive(true);
    }

    public void OpenCredits()
    {
        canvasMainMenu.SetActive(false);
        canvasCredits.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        canvasMainMenu.SetActive(true);
        canvasSettings.SetActive(false);
        canvasCredits.SetActive(false);
    }
}
