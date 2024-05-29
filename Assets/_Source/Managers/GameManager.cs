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
    bool isPlayerAlive = true;

    public delegate void GameManagerReadyHandler();
    public static event GameManagerReadyHandler OnGameManagerReady;

    private bool isMapReady = false;
    private bool isNavMeshReady = false;
    
    private void Awake()
    {
        SlotPlacer.OnMapGenerated += OnMapGenerated;
        NavMeshSurfaceManager.OnNavMeshGenerated += OnNavMeshGenerated;
        

        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    
    void OnDestroy()
    {
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
            OnGameManagerReady?.Invoke();
        }
    }

    void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    

    public PlayerController GetPlayerController() { return _playerController; }
    public Transform GetMainBaseTransform() {  return mainBaseTransform; }
    public void SetMainBaseTransform(Transform mainBase) {  mainBaseTransform = mainBase; }
    public bool IsPlayerAlive() {  return isPlayerAlive; }


}
