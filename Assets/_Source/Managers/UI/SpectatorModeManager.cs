using UnityEngine;

public class SpectatorModeManager : MonoBehaviour
{
    private static SpectatorModeManager _instance;
    public static SpectatorModeManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    
    public void StartSpectatorMode()
    {
        InputManager.Instance.StartDayCycle();
        HUDManager.Instance.StartSpectatorMode();
        CameraManager.Instance.StartSpectatorMode();
        PlayerController player = GameManager.Instance.GetPlayerController();
        MainBase main = GameManager.Instance.GetMainBaseTransform().GetComponent<MainBase>();
        player.transform.position = main.playerSpawnPoint.position;

        player.GetPlayerModel().SetActive(false);
        Time.timeScale = 2f;
    }

    public void EndSpectatorMode()
    {
        Time.timeScale = 1f;
    }
}
