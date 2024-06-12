using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    public bool IsScreenActive { get; set; }
    public float slowdownDuration = 1.0f;
    public float appearDuration = 0.5f;
    public AnimationCurve appearCurve;


    private static DeathScreenManager _instance;
    public static DeathScreenManager Instance { get { return _instance; } }

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

        deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        AudioManager.Instance.EnableMusicLowPassFilter(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        InputManager.Instance.DisableControllers();
        if (!IsScreenActive)
        {
            StartCoroutine(DisplayAnim());
        }

        IsScreenActive = true;

        if (PauseManager.Instance.IsGamePaused())
            PauseManager.Instance.PauseGame();
    }

    public void EnterDayMode()
    {
        AudioManager.Instance.EnableMusicLowPassFilter(false);
        if (!DayNightCycleManager.Instance.IsDay)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        deathScreen.SetActive(false);
        Time.timeScale = 1f;
        InputManager.Instance.EnableControllers();
        IsScreenActive = false;
    }

    private IEnumerator DisplayAnim()
    {
        float elapsedTime = 0;
        while (elapsedTime < slowdownDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp01(1.0f - (elapsedTime / slowdownDuration));
            yield return null;
        }

        Time.timeScale = 0.0f;
        Cursor.visible = true;
        deathScreen.SetActive(true);
        deathScreen.transform.localScale = new Vector3(2f, 2f, 2f);
        elapsedTime = 0;

        List<float> defaultOpacities = StoreDefaultOpacity(deathScreen);
        while (elapsedTime < appearDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentile = appearCurve.Evaluate(Mathf.Clamp01(elapsedTime / appearDuration));
            deathScreen.transform.localScale = new Vector3(2f - percentile, 2f - percentile, 2f - percentile);
            SetOpacity(deathScreen, percentile, defaultOpacities);
            yield return null;
        }
    }

    public List<float> StoreDefaultOpacity(GameObject parent)
    {
        List<float> defaultOpacities = new List<float>();
        foreach (Graphic graphic in parent.GetComponentsInChildren<Graphic>())
        {
            defaultOpacities.Add(graphic.color.a);
        }
        return defaultOpacities;
    }

    public void SetOpacity(GameObject parent, float percent, List<float> defaults)
    {
        int index = 0;
        foreach (Graphic graphic in parent.GetComponentsInChildren<Graphic>())
        {
            Color color = graphic.color;
            color.a = percent * defaults[index];
            graphic.color = color;
            index++;
        }
    }
}

