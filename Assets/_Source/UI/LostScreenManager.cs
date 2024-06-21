using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LostScreenManager : MonoBehaviour
{

    private static LostScreenManager _instance;
    public static LostScreenManager Instance { get { return _instance; } }

    [Foldout("References")]
    [SerializeField]
    GameObject lostScreenGameObject;

    float timer;
    bool isBaseDead = false;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;

        }
        _instance = this;
    }

    public void EnableLostScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lostScreenGameObject.SetActive(true);
        Time.timeScale = 0f;
        InputManager.Instance.DisableAllTimeController();
        InputManager.Instance.DisableControllers();

    }

}
