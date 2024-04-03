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


    private void Start()
    {
        EndNightCycle();        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            EndNightCycle();

        }
    }



    public void EndDayCycle()
    {
        DayEnd?.Invoke();
        NightBegin?.Invoke();
    }

    public void EndNightCycle()
    {
        NightEnd?.Invoke();
        DayBegin?.Invoke();
    }

}
