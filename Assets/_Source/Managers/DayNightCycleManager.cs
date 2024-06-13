using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DayNightCycleManager : MonoBehaviour
{

    private static DayNightCycleManager _instance;
    public static DayNightCycleManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public static Action DayBegin;
    public static Action DayEnd;


    public static event Action NightBegin;
    public static event Action NightEnd;

    public bool IsDay;
    private void Start()
    {
        EndNightCycle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {

            EndNightCycle();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {

            EndDayCycle();
        }
    }

    public void EndDayCycle()
    {
        IsDay = false;
        DayEnd?.Invoke();
        NightBegin?.Invoke();
        AudioManager.Instance.OnStartNight();

    }

    public void EndNightCycle()
    {
        IsDay = true;
        NightEnd?.Invoke();
        DayBegin?.Invoke();
        AudioManager.Instance.OnStartDay();
    }
}
