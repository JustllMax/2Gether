using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSettings;
    public GameObject panelCredits;

    void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelCredits.activeSelf || panelSettings.activeSelf)
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
        panelMainMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void OpenCredits()
    {
        panelMainMenu.SetActive(false);
        panelCredits.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
    }
}
