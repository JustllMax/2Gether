using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SpectatorModeManager : MonoBehaviour
{
    private static SpectatorModeManager _instance;
    public static SpectatorModeManager Instance { get { return _instance; } }

    public static Action SpectatorModeOn;

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
        if(SpectatorModeOn != null)
        {
            SpectatorModeOn.Invoke();
        }
    }
}
