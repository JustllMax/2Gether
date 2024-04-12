using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private float _cameraSensitivity = 0.1f;

    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _jumpStrength = 1.0f;


    private Rigidbody _objectRigidbody;

    private PlayerInputAction.FPSControllerActions FPSController;


    private bool _doubleJump = true;
    private bool _onGround = false;
    private bool _isDashing = false;
    private short _dashCount = 2;
    private Vector3 _lastMovement;
    private CharacterController _characterController;
    private AudioSource _audioSource;

    private Vector3 playerVelocity;

    [SerializeField]
    private float gravityValue = -9.81f;

    private float _cameraAngleX;

    private void Awake()
    {
        Application.targetFrameRate = 300;
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;

        FPSController.Jump.performed += OnJump;
        FPSController.Dash.performed += OnDash;
    }

    private void FixedUpdate()
    {
        //On ground check
        _onGround = _characterController.isGrounded;

        //Reset jump & dash
        if (_onGround)
        {
            _doubleJump = true;
            _dashCount = 2;
            if(playerVelocity.y < 0)
                playerVelocity.y = 0f;
        }

        //Movement
        Vector2 input = FPSController.Movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0f, input.y);
        move = transform.forward * move.z + transform.right * move.x;
        _lastMovement = move;

        playerVelocity.y += gravityValue * Time.deltaTime;

        _characterController.Move(move * _movementSpeed);
        _characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Update()
    {
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        var mouseInput = FPSController.Look.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, mouseInput.x * _cameraSensitivity, 0));

        _cameraAngleX += mouseInput.y * _cameraSensitivity;
        _cameraAngleX = Mathf.Clamp(_cameraAngleX, -90, 90);

        _camera.localRotation = Quaternion.Euler(new Vector3(-_cameraAngleX, 0, 0));
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_onGround)
        {
            _doubleJump = true;
        }

        if (_doubleJump)
        {
            _doubleJump = false;
        }
        else
        {
            return;
        }

        playerVelocity.y = Mathf.Sqrt(_jumpStrength * -3.0f * gravityValue);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_dashCount > 0 && !_isDashing)
        {
            _ = Dash();
            _dashCount--;
        }
    }

    private async UniTaskVoid Dash()
    {
        _isDashing = true;
        Vector3 movement = _lastMovement;
        if (movement == Vector3.zero)
        {
            movement = _camera.transform.forward;
            movement.y = 0f;
        }

        //_audioSource.Play();


        //Camera cam = _camera.GetComponent<Camera>();
        //float fov = cam.fieldOfView;

        float time = Time.time + 0.15f;
        while (Time.time < time)
        {
            float signal = Mathf.Sin((Time.time - time) / 0.15f * Mathf.PI) * 7.5f;
            //cam.fieldOfView = fov - signal;

            _characterController.Move(movement * 12f * Time.deltaTime);
            //_objectRigidbody.MovePosition(_objectRigidbody.position + movement * 12f * Time.deltaTime);
            await UniTask.Yield();
        }


       // cam.fieldOfView = fov;
        _isDashing = false;
    }
}
