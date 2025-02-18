using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ITargetable, IDamagable
{
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip doubleJumpSound;
    [SerializeField] AudioClip dashReadySound;
    [SerializeField] AudioClip dashEmptySound;

    [SerializeField]
    GameObject playerModel;
    [SerializeField]
    public float _maxHealth = 100f;
    [SerializeField]
    float _health;
    [SerializeField]
    [Range(0.1f, 3f)]
    float invincibilityDuration = 0.4f;

    [SerializeField]
    private Transform _nightCamera;

    [SerializeField]
    private Transform _gunCamera;

    [SerializeField]
    private float _cameraSensitivity = 0.1f;

    public float CurrentSensitivity;

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
    private ushort _maxDashCount = 3;

    [SerializeField]
    private bool _isMoving;

    [SerializeField]
    Animator _animator;

    private PlayerInputAction.FPSControllerActions FPSController;
    private CharacterController _characterController;
    private AudioSource _audioSource;

    private bool _doubleJump = true;
    private bool _onGround = false;
    private bool _isDashing = false;
    private ushort _dashCount;
    private float _dashCooldownTimer;
    private Vector3 _lastMovementDir;
    private float _cameraAngleX;
    [SerializeField]
    private bool canBeHit = true;
    public bool CanMove { get; set; } = true;
    public bool IsTargetable { get; set; }
    public TargetType TargetType { get; set; }
    public float DesiredSensivity { get => _cameraSensitivity; }
    public float Health { get => _health; set => _health = value; }

    public Vector3 Velocity { get => _velocity; set => _velocity = value; }


    #region SetUp
    private void Awake()
    {
        SetUpPlayer();
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        CurrentSensitivity = _cameraSensitivity;
    }

    private void OnEnable()
    {
        DayNightCycleManager.DayEnd += OnDayEnd;
    }

    private void OnDisable()
    {
        DayNightCycleManager.DayEnd -= OnDayEnd;
    }
    private void OnValidate()
    {
        if (TargetType != TargetType.Player)
            TargetType = TargetType.Player;
    }

    void Start()
    {
        FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;

        FPSController.Jump.performed += OnJump;
        SetUpHUD();
    }

    private void SetUpPlayer()
    {
        canBeHit = true;
        CanMove = true;
        Health = _maxHealth;
        IsTargetable = true;
        TargetType = TargetType.Player;
        _dashCount = _maxDashCount;
    }

    private void SetUpHUD()
    {
        HUDManager.Instance.SetMaxHealth(Health);
        HUDManager.Instance.SetCurrentHealth(Health);

        HUDManager.Instance.SetAllDashesMaxTimer(_dashCooldown);
        for (int i = 0; i < _maxDashCount; i++)
        {
            HUDManager.Instance.SetDashCurrentTimer(i, _dashCooldown);

        }
    }



    #endregion
    private void FixedUpdate()
    {
        _onGround = _characterController.isGrounded;
    }

    public void Update()
    {
        if (transform.position.y < -50f)
        {
            Kill();
        }

        if(CanMove == false)
        {
            return;
        }

        RotateCharacter();

        //Gravity
        _velocity += _gravityAcceleration * Time.deltaTime;

        //Dash cooldown
        if (!_isDashing && _dashCount < _maxDashCount && _dashCooldownTimer < _dashCooldown)
        {
            _dashCooldownTimer += Time.deltaTime;
            HUDManager.Instance.SetDashCurrentTimer(_dashCount, _dashCooldownTimer);
        }

        //Reset jump & dash
        if (_onGround)
        {
            _doubleJump = true;
            if (_velocity.y < 0)
                _velocity.y = 0f;

            if (_dashCooldownTimer >= _dashCooldown)
            {
                AudioManager.Instance.PlaySFXAtSource(dashReadySound, _audioSource, 1f + (((float)_dashCount-1)/_maxDashCount) * 0.25f);
                _dashCount++;
                _dashCooldownTimer = 0.0f;
            }
        }

        if (FPSController.Dash.WasPressedThisFrame())
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
        transform.Rotate(new Vector3(0, mouseInput.x * CurrentSensitivity, 0));

        _cameraAngleX += mouseInput.y * CurrentSensitivity;
        _cameraAngleX = Mathf.Clamp(_cameraAngleX, -90, 90);

        _nightCamera.localRotation = Quaternion.Euler(new Vector3(-_cameraAngleX, 0, 0));

        if(_gunCamera != null && _gunCamera.gameObject.active)
        {
            _gunCamera.rotation = _nightCamera.rotation;
        }
        

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_onGround)
        {
            AudioManager.Instance.PlaySFXAtSource(jumpSound, _audioSource);
        } else if (_doubleJump)
        {
            AudioManager.Instance.PlaySFXAtSource(doubleJumpSound, _audioSource);
        }
        else
        {
            return;
        }

        _doubleJump = false;
        float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(_gravityAcceleration.magnitude) * _jumpHeight);
        _velocity.y = 0;
        _velocity += _gravityAcceleration.normalized * -1 * jumpSpeed;
    }

    private void OnDash()
    {
        if (_dashCount > 0)
        {
            if (!_isDashing)
            {
                Dash();
                //_dashCooldownTimer = 0f;
                _dashCount--;
                HUDManager.Instance.SetDashCurrentTimer(_dashCount, _dashCooldownTimer);
                AudioManager.Instance.PlaySFXAtSource(dashSound, _audioSource);
            }
        } 
        else
        {
            AudioManager.Instance.PlaySFXAtSource(dashEmptySound, _audioSource);
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
            movement = _nightCamera.transform.forward;
            movement.y = 0f;
        }
        movement = movement.normalized;

        Vector3 lastVelocity = _velocity;
        float time = Time.time + 0.15f;
        while (Time.time < time)
        {
            _velocity.x = movement.x * _dashSpeed;
            _velocity.z = movement.z * _dashSpeed;
            await UniTask.Yield();
        }

        _velocity = lastVelocity;
        if (_velocity.y < 0f)
            _velocity.y = 0;

        time = Time.time + 0.05f;
        while (Time.time < time)
        {
            await UniTask.Yield();
        }
        _isDashing = false;
    }

    private async UniTaskVoid InvincibilityFramesCounter()
    {
        await UniTask.WaitForSeconds(invincibilityDuration);

        canBeHit = true;
    }

    public bool TakeDamage(float damage)
    {
        if (!canBeHit)
            return false;

        canBeHit = false;
        Debug.Log(this + "took " + damage + " damage");
        Health -= damage;
        HUDManager.Instance.PlayerGotHit(invincibilityDuration);
        HUDManager.Instance.SetCurrentHealth(Health);
        if(Health <= 0)
        {
            Kill();
            return true;
        }
        _ = InvincibilityFramesCounter();
        return false;
    }

    public bool Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, _maxHealth);
        HUDManager.Instance.SetCurrentHealth(Health);
        HUDManager.Instance.PlayerGotHealed(invincibilityDuration);

        return true;
    }

    public void Kill()
    {
        IsTargetable = false;
        CanMove = false;
        
        GameManager.Instance.OnPlayerDeathInvoke();

        StartCoroutine(DeathAnimation());
        return;
    }

    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        DeathScreenManager.Instance.ShowDeathScreen();

        float elapsedTime = 0;
        float deathTime = 0.75f;
        while (elapsedTime < deathTime)
        {
            elapsedTime += Time.deltaTime;
            float angle = Mathf.Clamp01(elapsedTime / deathTime) * 90;

            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(euler.x, euler.y, angle));
            transform.position += new Vector3(0, -0.8f * Time.deltaTime, 0);
            yield return null;
        }
    }
    public void SetCameraFOV(float value) 
    {
        Camera.main.fieldOfView = value;
    }

    public GameObject GetPlayerModel()
    {
        return playerModel;
    }

    public void OnDayEnd()
    {
        SetUpPlayer();
        SetUpHUD();
        transform.rotation = Quaternion.identity;
        playerModel.SetActive(true);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.attachedRigidbody != null && hit.collider.attachedRigidbody.CompareTag("Enemy"))
        {
            Vector3 normal = hit.normal;
            if (Vector3.Dot(normal, Vector3.up) > 0.5f)
            {
                Vector3 offset = hit.point - hit.collider.attachedRigidbody.transform.position;
                Vector3 velocity = new Vector3(Mathf.Clamp(1.5f/offset.x, -5, 5), 0, Mathf.Clamp(1.5f / offset.z, -5, 5));

                _velocity = Vector3.Lerp(_velocity, velocity, Time.fixedDeltaTime * 50 * _groundControlFactor);
            }
        }
    }
}
