using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour, ITargetable, IDamagable
{

    [SerializeField]
    float _health = 100f;

    [SerializeField]
    private Transform _camera;

    [SerializeField]
    private Transform gunCamera;

    [SerializeField]
    private float _cameraSensitivity = 0.1f;

    [SerializeField]
    private Vector3 _velocity;

    [SerializeField]
    private Vector3 _gravityAcceleration = new Vector3(0f,-9.81f,0f);

    [SerializeField]
    private float _groundDrag = 0.4f;

    [SerializeField]
    private float _groundControlFactor = 0.35f;

    [SerializeField]
    private float _airControlFactor = 0.1f;

    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _jumpHeight = 1.0f;

    [SerializeField]
    private float _dashSpeed = 10.0f;

    [SerializeField]
    private float _dashCooldown = 1.0f;

    [SerializeField]
    private ushort _maxDashCount = 2;

    [SerializeField]
    private bool _isMoving;

    private PlayerInputAction.FPSControllerActions FPSController;
    private CharacterController _characterController;
    private AudioSource _audioSource;

    private bool _doubleJump = true;
    private bool _onGround = false;
    private bool _isDashing = false;
    private ushort _dashCount;
    private float _dashCooldownTime;
    private Vector3 _lastMovementDir;
    private float _cameraAngleX;

    public bool IsTargetable { get; set; }
    public TargetType TargetType { get; set; }
    public float Health { get; set; }

    private void Awake()
    {
        Health = _health;
        IsTargetable = true;
        TargetType = TargetType.Player;
        Application.targetFrameRate = 300;
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _dashCount = _maxDashCount;
    }

    void Start()
    {
        FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;

        FPSController.Jump.performed += OnJump;
    }

    private void FixedUpdate()
    {
        _onGround = _characterController.isGrounded;
    }

    public void Update()
    {
        RotateCharacter();

        //Gravity
        _velocity += _gravityAcceleration * Time.deltaTime;

        //Dash cooldown
        if (!_isDashing && _dashCount < _maxDashCount && _dashCooldownTime < _dashCooldown)
        {
            _dashCooldownTime += Time.deltaTime;
        }

        //Reset jump & dash
        if (_onGround)
        {
            _doubleJump = true;
            if (_velocity.y < 0)
                _velocity.y = 0f;

            if (_dashCooldownTime >= _dashCooldown)
            {
                _dashCount = _maxDashCount;
                _dashCooldownTime = 0.0f;
            }
        }

        if (FPSController.Dash.IsPressed())
            OnDash();

        if (!_isDashing)
        {
            //Movement
            Vector2 input = FPSController.Movement.ReadValue<Vector2>();
            _lastMovementDir = (transform.forward * input.y + transform.right * input.x).normalized;

            _isMoving = input == Vector2.zero ? false : true;

            if (input != Vector2.zero)
            {
                Vector3 movement = _lastMovementDir * _movementSpeed;
                movement.y = _velocity.y;

                if (_onGround)
                    _velocity = Vector3.Lerp(_velocity, movement, Time.deltaTime * 50 * _groundControlFactor);
                else
                    _velocity = Vector3.Lerp(_velocity, movement, Time.deltaTime * 50 * _airControlFactor);
            }
            else if (_onGround)
            {
                //Ground drag
                _velocity.x = Mathf.Lerp(_velocity.x, 0, Time.deltaTime * 50 * _groundDrag);
                _velocity.z = Mathf.Lerp(_velocity.z, 0, Time.deltaTime * 50 * _groundDrag);
            }
        }

        _characterController.Move((_velocity - new Vector3(0,0.01f,0)) * Time.deltaTime);
    }

    private void RotateCharacter()
    {

        var mouseInput = FPSController.Look.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, mouseInput.x * _cameraSensitivity, 0));

        _cameraAngleX += mouseInput.y * _cameraSensitivity;
        _cameraAngleX = Mathf.Clamp(_cameraAngleX, -90, 90);

        _camera.localRotation = Quaternion.Euler(new Vector3(-_cameraAngleX, 0, 0));

        gunCamera.rotation = _camera.rotation;

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

        float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(_gravityAcceleration.magnitude) * _jumpHeight);
        _velocity.y = 0;
        _velocity += _gravityAcceleration.normalized * -1 * jumpSpeed;
    }

    private void OnDash()
    {
        if (_dashCount > 0 && !_isDashing)
        {
            Dash();
            _dashCount--;
        }
    }

    private async UniTaskVoid Dash()
    {
        if (!_isMoving)
            return;

        _isDashing = true;

        Vector3 movement = _lastMovementDir;
        if (movement == Vector3.zero)
        {
            movement = _camera.transform.forward;
            movement.y = 0f;
        }
        movement = movement.normalized;

        float time = Time.time + 0.15f;
        while (Time.time < time)
        {
            _velocity.x = movement.x * _dashSpeed;
            _velocity.z = movement.z * _dashSpeed;
            await UniTask.Yield();
        }
        _isDashing = false;
        return;

        _velocity = new Vector3(0,0,0);

        time = Time.time + 0.025f;
        while (Time.time < time)
        {
            await UniTask.Yield();
        }
        _isDashing = false;
    }

    public bool TakeDamage(float damage)
    {

        Health -= damage;
        if(Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public void Kill()
    {
        IsTargetable = false;
        
        return;
    }
}
