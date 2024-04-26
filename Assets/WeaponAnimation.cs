using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private PlayerInputAction.FPSControllerActions _fps;


    private Vector2 _lookInput;
    private Vector2 _moveInput;

    [Header("Sway")]
    [SerializeField]
    private float _step = 0.01f;
    [SerializeField]
    private float _maxStep = 0.06f;
    private Vector3 _swayPos;

    [SerializeField]
    private float _rotStep = 4f;
    [SerializeField]
    private float _maxRotStep = 5f;
    private Vector3 _swayRot;

    [SerializeField]
    private float _smooth = 10f;
    [SerializeField]
    private float _smoothRot = 12f;


    // Start is called before the first frame update
    void Start()
    {
        _fps = InputManager.Instance.GetPlayerInputAction().FPSController;
    }


    // Update is called once per frame
    void Update()
    {
        _lookInput = _fps.Look.ReadValue<Vector2>();

        Debug.Log(_lookInput);

        _moveInput = _fps.Movement.ReadValue<Vector2>();
        _moveInput.Normalize();

        Sway();
        ApplyTransform();
    }

    private void ApplyTransform()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _swayPos, Time.deltaTime * _smooth);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_swayRot), Time.deltaTime * _smooth);
    }

    private void Sway()
    {
        var invLook = _lookInput * -_step;
        invLook.x = Mathf.Clamp(invLook.x, -_maxStep, _maxStep);
        invLook.y = Mathf.Clamp(invLook.y, -_maxStep, _maxStep);

        _swayPos = invLook;

        invLook = _lookInput * -_rotStep;
        invLook.x = Mathf.Clamp(invLook.x, -_maxRotStep, _maxRotStep);
        invLook.y = Mathf.Clamp(invLook.y, -_maxRotStep, _maxRotStep);

        _swayRot = new Vector3(invLook.y, invLook.x, invLook.x);
    }
}
