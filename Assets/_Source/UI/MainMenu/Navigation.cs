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
        AudioManager.Instance.PlaySFX("A_UI_Correct_Button_Down");
        AudioManager.Instance.StopMusic("A_Menu_Music");
        SceneManager.LoadScene(1);
        //AudioManager.Instance.PlayAmbient("A_DayUI_Ambient");
        //AudioManager.Instance.PlayMusic("A_DayUI_Music");
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFX("A_UI_Correct_Button_Down");
        panelMainMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void OpenCredits()
    {
        AudioManager.Instance.PlaySFX("A_UI_Correct_Button_Down");
        panelMainMenu.SetActive(false);
        panelCredits.SetActive(true);
    }

    public void ExitGame()
    {
        AudioManager.Instance.StopMusic("A_Menu_Music");
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        if (!AudioManager.Instance.IsMusicPlaying("A_Menu_Music"))
        {
            AudioManager.Instance.PlayMusic("A_Menu_Music");
        }
        AudioManager.Instance.EnableMusicLowPassFilter(false);
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
    }

}
