using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class DayTime : MonoBehaviour
{
    [SerializeField] Material daySkybox;
    [SerializeField] Material nightSkybox;

    [SerializeField] private Gradient directLightGradient;
    [SerializeField] private Gradient ambientLightGradient;
    [SerializeField, Range(1, 3600)] private float timeDayInSeconds = 60;
    [SerializeField, Range(0f, 1f)] private float timeProgress = 0.9f;

    [SerializeField] private Light dirLight;
    [SerializeField] private float _currDayTime = 0;
    [SerializeField, Range(0.3f, 0.7f)] private float _dayTime = 0.55f;
    [SerializeField, Range(0.5f, 0.99f)] private float _nightTime = 0.9f;
    [SerializeField, Range(0f, 0.1f)] private float tolerance = 0.1f;
    private Vector3 _defaultAngles;


    private void OnEnable()
    {
        DayNightCycleManager.NightBegin += SetNightLight;
        DayNightCycleManager.DayBegin += SetDayLight;
    }
    private void OnDisable()
    {
        DayNightCycleManager.NightBegin -= SetNightLight;
        DayNightCycleManager.DayBegin -= SetDayLight;
    }

    void Start()
    {
        _defaultAngles = dirLight.transform.localEulerAngles;
    }

    public void SetNightLight()
    {
        _currDayTime = _nightTime;
        RenderSettings.skybox = nightSkybox;
    }

    public void SetDayLight()
    {
        _currDayTime = _dayTime;
        RenderSettings.skybox = daySkybox;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeProgress > 1f)
        {
            timeProgress = 0f;
        }

        if (Mathf.Abs(timeProgress - _currDayTime) > tolerance)
        {
            timeProgress += Time.deltaTime / timeDayInSeconds;
        }

        dirLight.color = directLightGradient.Evaluate(timeProgress);
        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeProgress);

        dirLight.transform.localEulerAngles = new Vector3(360f * timeProgress - 45, _defaultAngles.y, _defaultAngles.z);
    }
}
