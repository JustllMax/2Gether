using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    PlayerInputAction playerInputAction;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        DayNightCycleManager.DayBegin += StartDayCycle;
        DayNightCycleManager.NightBegin += StartNightCycle;
    }
    private void OnDisable()
    {
        DayNightCycleManager.DayBegin -= StartDayCycle;
        DayNightCycleManager.NightBegin -= StartNightCycle;
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }

    public void StartDayCycle()
    {

        playerInputAction.FPSController.Disable();
        playerInputAction.BuilderController.Enable();
    }

    public void StartNightCycle()
    {
        playerInputAction.BuilderController.Disable();
        playerInputAction.FPSController.Enable();
    }
}
