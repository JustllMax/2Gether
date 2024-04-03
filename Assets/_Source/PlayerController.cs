using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;

    [SerializeField]
    private float _cameraSensitivity = 0.1f;

    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _jumpStrength = 1.0f;


    private Rigidbody _objectRigidbody;

    private InputAction _move;
    private InputAction _look;
    private Vector3 _cameraAngles;

    private bool _doubleJump = true;
    private bool _onGround = false;
    private bool _isDashing = false;
    private short _dashCount = 2;
    private Vector3 _lastMovement;

    private AudioSource _audioSource;

    void Start()
    {
        Application.targetFrameRate = 160;
        _objectRigidbody = GetComponent<Rigidbody>();
        _audioSource =  GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Controls

       // _move = _controls.Player.Move;
       // _look = _controls.Player.Look;
        //_controls.Player.Attack.performed += OnAttack;
        //_controls.Player.Fire.performed += OnFire;
       // _controls.Player.Exit.performed += OnExit;
        //_controls.Player.Jump.performed += OnJump;
        //_controls.Player.Dash.performed += OnDash;
    }
    void OnExit(InputAction.CallbackContext c)
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        //On ground check
        _onGround = IsGrounded();

        //Reset jump & dash
        if (_onGround)
        {
            _doubleJump = true;
            _dashCount = 2;
        }

        //Movement
        Vector2 input = _move.ReadValue<Vector2>() * -1.0f;

        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        _lastMovement = (cameraForward * input.y + cameraRight * input.x) * -1.0f;

        if (input != Vector2.zero)
        {
            Vector3 movement = _lastMovement * _movementSpeed;
            movement.y = _objectRigidbody.velocity.y;

            if(_onGround)
                _objectRigidbody.velocity = Vector3.Lerp(_objectRigidbody.velocity, movement, 0.35f);
            else
                _objectRigidbody.velocity = Vector3.Lerp(_objectRigidbody.velocity, movement, 0.25f);
        }
    }

    void Update()
    {
        //Camera
        Vector2 cameraVec = _look.ReadValue<Vector2>() * _cameraSensitivity;

        _cameraAngles.x -= cameraVec.y;
        _cameraAngles.x = Mathf.Clamp(_cameraAngles.x, -89.0f, 89.0f);
        _cameraAngles.y += cameraVec.x;
        _camera.transform.eulerAngles = _cameraAngles;
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

        _objectRigidbody.velocity = new Vector3(_objectRigidbody.velocity.x, 0f, _objectRigidbody.velocity.z);
        _objectRigidbody.AddForce(Vector3.up * _objectRigidbody.mass * _jumpStrength, ForceMode.Impulse);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_dashCount > 0 && !_isDashing)
        {
            StartCoroutine("Dash");
            _dashCount--;
        }
    }

    private IEnumerator Dash()
    {
        _isDashing = true;
        Vector3 movement = _lastMovement;
        if (movement == Vector3.zero)
        {
            movement = _camera.transform.forward;
            movement.y = 0f;
        }

        _audioSource.Play();


        Camera cam = _camera.GetComponent<Camera>();
        float fov = cam.fieldOfView;

        float time = Time.time + 0.15f;
        while (Time.time < time)
        {
            float signal = Mathf.Sin((Time.time - time) / 0.15f * Mathf.PI) * 7.5f;
            cam.fieldOfView = fov - signal;

            _objectRigidbody.velocity = new Vector3(0f, 0f, 0f);
            _objectRigidbody.MovePosition(_objectRigidbody.position + movement * 12f * Time.deltaTime);
            yield return null;
        }


        cam.fieldOfView = fov;
        _isDashing = false;
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position - new Vector3(0f, 0.6f, 0f), 0.35f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject)
                return true;
        }
        return false;
    }
}
