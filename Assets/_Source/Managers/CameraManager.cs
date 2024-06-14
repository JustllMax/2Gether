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
    private GameObject groundFog;

    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
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

    private void StartDayCycle()
    {
        SwitchCameraPriority(dayCamera, 20);
        SwitchCameraPriority(nightCamera, 10);
        UpdateCrosshairVisibility();
        UpdateFogSystemVisibility();
    }
    private void StartNightCycle()
    {
        SwitchCameraPriority(dayCamera, 10);
        SwitchCameraPriority(nightCamera, 20);
        UpdateCrosshairVisibility();
        UpdateFogSystemVisibility();
    }

    private void SwitchCameraPriority(CinemachineVirtualCamera camera, int priority)
    {
        if (camera != null)
        {
            camera.Priority = priority;
            
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
            GunCameraObj.SetActive(true);

        }
        else if(!isNightCameraActive) //Day Camera
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GunCameraObj.SetActive(false);
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

    public void StartSpectatorMode()
    {
        StartDayCycle();
    }
}