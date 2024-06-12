using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandIKController : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private WeaponIKConfig _activeConfig;

    [SerializeField]
    private TwoBoneIKConstraint _leftHandConstraint;

    [SerializeField]
    private TwoBoneIKConstraint _rightHandConstraint;


    [SerializeField]
    private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;


    public void ChangeActiveConfig(WeaponIKConfig conf)
    {
        _activeConfig = conf;
    }

    private void Update()
    {
        if (!_activeConfig)
            return;

        _leftHandTarget.position = _activeConfig.LeftHandTarget.position;
        _leftHandTarget.rotation = _activeConfig.LeftHandTarget.rotation;

        _rightHandTarget.position = _activeConfig.RightHandTarget.position;
        _rightHandTarget.rotation = _activeConfig.RightHandTarget.rotation;
        
    }
}
