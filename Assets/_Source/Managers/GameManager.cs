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
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    public PlayerController GetPlayerController() { return _playerController; }
    public Transform GetMainBaseTransform() {  return mainBaseTransform; }
    public bool IsPlayerAlive() {  return isPlayerAlive; }


}
