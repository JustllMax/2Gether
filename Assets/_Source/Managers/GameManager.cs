using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] PlayerController _playerController;
    [SerializeField] Transform mainBaseTransform;

    //TODO: Change to event 
    public bool isPlayerAlive = true;
    public static Action OnPlayerDeath;
    public delegate void GameManagerReadyHandler();
    public static event GameManagerReadyHandler OnGameManagerReady;
    private bool isMapReady = false;
    private bool isNavMeshReady = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    void OnEnable()
    {
        DayNightCycleManager.NightBegin += OnNightStart;
        DayNightCycleManager.NightEnd += OnNightEnd;
        SpectatorModeManager.SpectatorModeOn += OnSpectatorModeStart;
        SlotPlacer.OnMapGenerated += OnMapGenerated;
        NavMeshSurfaceManager.OnNavMeshGenerated += OnNavMeshGenerated;

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }
    private void Start()
    {
        Application.targetFrameRate = 300;
    }
    void OnDisable()
    {
        DayNightCycleManager.NightBegin += OnNightStart;
        SpectatorModeManager.SpectatorModeOn += OnSpectatorModeStart;
        SlotPlacer.OnMapGenerated -= OnMapGenerated;
        NavMeshSurfaceManager.OnNavMeshGenerated -= OnNavMeshGenerated;
    }
    void OnMapGenerated()
    {
        isMapReady = true;
        mainBaseTransform = SlotPlacer.Instance.spawnedSlots[SlotPlacer.Instance.startPos.x, SlotPlacer.Instance.startPos.y].transform;
        CheckIfReady();
    }

    void OnNavMeshGenerated()
    {
        isNavMeshReady = true;
        CheckIfReady();
    }

    void CheckIfReady()
    {
        if (isMapReady && isNavMeshReady)
        {
            Debug.Log("GameManager Invoke");
            OnGameManagerReady?.Invoke();
        }
    }

    public PlayerController GetPlayerController() { return _playerController; }

    public Transform GetMainBaseTransform() { return mainBaseTransform; }
    public void SetMainBaseTransform(Transform mainBase) { mainBaseTransform = mainBase; }
    public bool IsPlayerAlive() { return isPlayerAlive; }

    public void OnPlayerDeathInvoke()
    {
        isPlayerAlive = false;
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    void OnSpectatorModeStart()
    {
        InputManager.Instance.StartDayCycle();
        _playerController.transform.position = mainBaseTransform.GetComponent<MainBase>().playerSpawnPoint.position;
        _playerController.GetPlayerModel().SetActive(false);
        Time.timeScale = 2f;
    }

    void OnNightStart()
    {
        _playerController.GetPlayerModel().SetActive(true);
    }
    void OnNightEnd()
    {
        Time.timeScale = 1f;
    }
}
