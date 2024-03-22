using UnityEngine;
using Cinemachine;

public class CameraPriorityController : MonoBehaviour
{
    [SerializeField] 
    private CinemachineVirtualCamera nightCamera;
    [SerializeField]
    private CinemachineVirtualCamera dayCamera;
    [SerializeField] 
    private GameObject crosshairUI;


    private void Start()
    {
        // Sets starting state of crosshair UI depending on the initial camera priority
        UpdateCrosshairVisibility();
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
        // We assume that the night camera has a higher priority to hide the crosshair
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