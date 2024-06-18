using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LampLightManager : MonoBehaviour
{
    [SerializeField] private float _intensity = 70f;
    private float timeProgress = 0f;
    [SerializeField, Range(0f, 100f)] private float lightChangingTime = 80f;
    private Light lighting;
    void Awake()
    {
        lighting = gameObject.GetComponent<Light>();
    }
    private void FixedUpdate()
    {
        if (!DayNightCycleManager.Instance.IsDay)
        {
            if (timeProgress < _intensity)
            {
                timeProgress += Time.deltaTime * lightChangingTime;
                lighting.intensity = timeProgress;
            }
        }
        if (DayNightCycleManager.Instance.IsDay)
        {
            if (timeProgress > 0)
            {
                timeProgress -= Time.deltaTime * lightChangingTime;
                lighting.intensity = timeProgress;
            }
        }
    }
}
