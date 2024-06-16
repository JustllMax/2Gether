using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LampLightManager : MonoBehaviour
{
    void OnEnable()
    {   
        DayNightCycleManager.NightEnd += TurnOffLampLight;
        DayNightCycleManager.DayEnd += TurnOnLampLight;
    }
    void OnDisable()
    {
        DayNightCycleManager.DayBegin -= TurnOffLampLight;
        DayNightCycleManager.NightBegin -= TurnOnLampLight;
    }
    public void TurnOnLampLight()
    {
       
        gameObject.SetActive(true);
    }
    public void TurnOffLampLight()
    {
        gameObject.SetActive(false);
    }
}
