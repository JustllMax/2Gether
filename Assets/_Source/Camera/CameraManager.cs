using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera dayCamera;
    [SerializeField] 
    private CinemachineVirtualCamera nightCamera;
    [SerializeField] 
    private GameObject crosshairUI;


    private void Start()
    {
        UpdateCrosshairVisibility();
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

    private void Update()
    {
        // Change map to nightcycle
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SwitchCameraPriority(dayCamera, 10);
            SwitchCameraPriority(nightCamera, 20);
        }
        // Change map to daycycle
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            SwitchCameraPriority(dayCamera, 20);
            SwitchCameraPriority(nightCamera, 10);
        }
    }

    private void StartDayCycle()
    {
        SwitchCameraPriority(dayCamera, 20);
        SwitchCameraPriority(nightCamera, 10);
    }
    private void StartNightCycle()
    {
        SwitchCameraPriority(dayCamera, 10);
        SwitchCameraPriority(nightCamera, 20);
    }

    private void SwitchCameraPriority(CinemachineVirtualCamera camera, int priority)
    {
        if (camera != null)
        {
            camera.Priority = priority;
            UpdateCrosshairVisibility();
        }
    }

    private void UpdateCrosshairVisibility()
    {
        // We assume that the night camera has a higher priority to hide the cursor
        bool isNightCameraActive = nightCamera != null && nightCamera.Priority > dayCamera.Priority;

        if (isNightCameraActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshairUI.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            crosshairUI.SetActive(false);
        }
    }
}