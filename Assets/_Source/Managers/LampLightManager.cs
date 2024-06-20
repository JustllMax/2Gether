using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LampLightManager : MonoBehaviour
{
    [SerializeField] private float _shadowDistance = 30f;
    [SerializeField] private float _intensity = 70f;
    private float timeProgress = 0f;
    [SerializeField, Range(0f, 100f)] private float lightChangingTime = 80f;
    private Light lighting;
    void Awake()
    {
        lighting = gameObject.GetComponent<Light>();

    }
    private void Start()
    {
        Debug.Log(this + " is turned off until it gets optimised, game is unplayable because of lag");
    }

    private void FixedUpdate()
    {
        if(DayNightCycleManager.Instance == null)
        {
            return;
        }

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


        if (Vector3.Distance(Camera.main.transform.position, transform.position) < _shadowDistance)
        {
            if (lighting.shadows != LightShadows.Soft)
                lighting.shadows = LightShadows.Soft;
        }
        else
        {
            if (lighting.shadows != LightShadows.None)
                lighting.shadows = LightShadows.None;
        }

    }
}
