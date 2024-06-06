using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject GunCameraObj;
    [SerializeField]
    private CinemachineVirtualCamera dayCamera;
    [SerializeField]
    private CinemachineVirtualCamera nightCamera;
    [SerializeField]
    private GameObject crosshairUI;
    [SerializeField]
    private GameObject groundFog;

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
            UpdateFogSystemVisibility();
        }
    }

    private void UpdateCrosshairVisibility()
    {
        // We assume that the night camera has a higher priority to hide the cursor
        bool isNightCameraActive = nightCamera != null && dayCamera != null && nightCamera.Priority > dayCamera.Priority;

        if (isNightCameraActive) //Night Camera
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GunCameradDisplayUI.SetActive(true);
            crosshairUI.SetActive(true);
            GunCameraObj.SetActive(true);
            //Fire event that camera changed to night

        }
        else //Day Camera
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            crosshairUI.SetActive(false);
            GunCameradDisplayUI.SetActive(false);
            GunCameraObj.SetActive(false);
            //Fire event that camera changed to day
        }
    }
    private void UpdateFogSystemVisibility()
    {
        // Fog only active in night
        if (groundFog != null)
        {
            groundFog.SetActive(nightCamera.Priority > dayCamera.Priority);
        }
    }
}