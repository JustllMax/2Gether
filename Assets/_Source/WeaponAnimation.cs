using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private PlayerInputAction.FPSControllerActions _fps;
    private CharacterController _cc;

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


    [Header("Bobbing")]
    public float speedCurve;
    float _curveSin { get => Mathf.Sin(speedCurve); }
    float _curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 _bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 _bobEulerRotation;

    [Header("Customization")]
    [SerializeField]
    private Vector3 _rotationOffsetEuler;

    // Start is called before the first frame update
    void Start()
    {
        _fps = InputManager.Instance.GetPlayerInputAction().FPSController;
        _cc = GetComponentInParent<CharacterController>();
    }

    void Bobbing()
    {
        // pos
        speedCurve += Time.deltaTime * (_cc.isGrounded ? (_lookInput.x + _lookInput.y) * bobExaggeration : 1f) + 0.01f;

        _bobPosition.x = 
            (_curveCos * bobLimit.x - (_moveInput.x * travelLimit.x));

        _bobPosition.y = (_curveSin * bobLimit.y) - (_lookInput.y * travelLimit.y);
        _bobPosition.z = -(_moveInput.y * travelLimit.z);

        //rot 

        _bobEulerRotation.x = (_moveInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * Mathf.Sin(2 * speedCurve) / 2);
        _bobEulerRotation.y = (_moveInput != Vector2.zero ? multiplier.y * _curveCos : 0);
        _bobEulerRotation.z = (_moveInput != Vector2.zero ? multiplier.z * _curveCos * _moveInput.x : 0);

    }

    // Update is called once per frame
    void Update()
    {
        _lookInput = _fps.Look.ReadValue<Vector2>();

        _moveInput = _fps.Movement.ReadValue<Vector2>();
        _moveInput.Normalize();

        Sway();
        Bobbing();

        ApplyTransform();
    }

    private void ApplyTransform()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _swayPos + _bobPosition, Time.deltaTime * _smooth);



        transform.localRotation = 
            Quaternion.Slerp(transform.localRotation, 
            Quaternion.Euler(_rotationOffsetEuler) * Quaternion.Euler(_swayRot) * Quaternion.Euler(_bobEulerRotation), Time.deltaTime * _smooth);
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
