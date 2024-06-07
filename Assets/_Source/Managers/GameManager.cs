using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] PlayerController _playerController;
    [SerializeField] Transform mainBaseTransform;
    [SerializeField] UIFlow _cardManager;

    public CardPoolData InitialCardPool;

	//TODO: Change to event 
    public bool isPlayerAlive = true;

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
        Application.targetFrameRate = 300;
    }
    private void OnEnable()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        StartGame();
    }

    public void StartGame()
    {
        // 1. display the ui
        _cardManager.ShowPanel(InitialCardPool);
    }

    public PlayerController GetPlayerController() { return _playerController; }
    public Transform GetMainBaseTransform() {  return mainBaseTransform; }
    public bool IsPlayerAlive() {  return isPlayerAlive; }

    public void OnPlayerDeath()
    {
        isPlayerAlive = false;
    }
}
