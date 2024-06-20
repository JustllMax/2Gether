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
           
        }
        else
        {
            _instance = this;

        }
        playerInputAction = new PlayerInputAction();
        playerInputAction.AllTime.Enable();
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
        Debug.Log(this + " fps disabled");

    }

    public void StartNightCycle()
    {
        playerInputAction.BuilderController.Disable();
        playerInputAction.FPSController.Enable();
        Debug.Log(this + " fps enabled");
    }

    public void DisableControllers()
    {
        playerInputAction.BuilderController.Disable();
        playerInputAction.FPSController.Disable();
    }

    public void EnableControllers()
    {
        playerInputAction.BuilderController.Enable();
        playerInputAction.FPSController.Enable();
    }

    public void EnableFPSController()
    {
        playerInputAction.FPSController.Enable();
    }

    public void DisableFPSController()
    {
        playerInputAction.FPSController.Disable();
    }

    public void EnableBuilderController()
    {
        playerInputAction.BuilderController.Enable();
    }

    public void DisableBuilderController()
    {
        playerInputAction.BuilderController.Disable();
    }

    public void DisableAllTimeController()
    {
        playerInputAction.AllTime.Disable();
    }
}
