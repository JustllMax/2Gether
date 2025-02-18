using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerGunController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] CinemachineVirtualCamera nightCamera;
    [SerializeField] Transform FirePoint;
    PlayerInputAction.FPSControllerActions _FPScontroller;
    PlayerEquipment _equipment;

    [SerializeField]
    Animator _animator;

    bool firedButtonHeld = false;
    float buttonHeldTimer = 0;
    bool isHoldingFire = false;
    [SerializeField]
    bool isDuringAnimation = false;
    bool flagOneShot = true;

    private void Awake()
    {
        _equipment = GetComponentInChildren<PlayerEquipment>();
    }

    void Start()
    {

        _FPScontroller = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPScontroller.Fire.performed += PerformFireWeapon;
        _FPScontroller.Fire.canceled += CancelFireWeapon;
        _FPScontroller.Aim.performed += PerformAimWeapon;
        _FPScontroller.Reload.performed += ReloadWeapon;

    }

    private void Update()
    {
        FirePoint.rotation = nightCamera.transform.rotation;

        CheckForIdleAnimation();



        if (firedButtonHeld )
        {
            CheckForHoldingFireButton();
            if (_equipment != null)
            {
                if (_equipment.CanFire())
                {
                    TryFiring();
                }

            }
        }

    }

    private void PerformFireWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && !isDuringAnimation) // the key has been pressed
        {
            firedButtonHeld = true;
        }        
    }

    private void CancelFireWeapon(InputAction.CallbackContext context)
    {
        if (context.canceled) //the key has been released
        {
            firedButtonHeld = false;
            buttonHeldTimer = 0f;
            isHoldingFire = false;
            flagOneShot = true;
        }
    }

    private void PerformAimWeapon(InputAction.CallbackContext context)
    {
        
        if(_equipment != null)
        {
            if (_equipment.CanAim())
            {
                _equipment.GetCurrentGun().Aim();
            }
        }
        
    }

    public void ReloadWeapon(InputAction.CallbackContext context)
    {
        
        if(_equipment != null)
        {
            if (_equipment.CanReload() && !isDuringAnimation)
            {
                if (!_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.RELOADDOWN.ToString()))
                {
                    _animator.CrossFade(PlayerAnimNames.RELOADDOWN.ToString(), 0.1f);
                }

            }
        }
    }

    public bool ReloadWeapon()
    {
        if (_equipment != null)
        {
            if (_equipment.CanReload() && !isDuringAnimation)
            {
                if (!_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.RELOADDOWN.ToString()))
                {
                    _animator.CrossFade(PlayerAnimNames.RELOADDOWN.ToString(), 0.5f);
                    return true;
                }

            }
        }
        return false;
    }

    void CheckForIdleAnimation()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimNames.IDLE.ToString()) || _animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimNames.SCOPE.ToString()))
        {
            isDuringAnimation = false;
        }
        else
        {
            isDuringAnimation = true;
        }
    }

    void CheckForHoldingFireButton()
    {
        buttonHeldTimer += Time.deltaTime;
        if (buttonHeldTimer > 0.01)
        {
            isHoldingFire = true;
        }
    }

    void TryFiring()
    {

        if (flagOneShot)
        {
            flagOneShot = false;
            if (_equipment.GetCurrentGun().Fire(false, FirePoint))
            {
                HUDManager.Instance.SetCurrentAmmo(_equipment.GetCurrentGun().GetAmmoInMagazine());
            }
        }
        else
        {
            if (_equipment.GetCurrentGun().Fire(isHoldingFire, FirePoint))
            {
                HUDManager.Instance.SetCurrentAmmo(_equipment.GetCurrentGun().GetAmmoInMagazine());
            }
        }
    }

    public bool StartScoping()
    {
        if (!_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.SCOPE.ToString()))
        {
            _animator.CrossFade(PlayerAnimNames.SCOPE.ToString(), 0.1f);
            return true;
        }
        return false;
    }

    public bool StartUnScoping()
    {
        if (!_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.UNSCOPE.ToString()))
        {
            _animator.CrossFade(PlayerAnimNames.UNSCOPE.ToString(), 0.1f);
            return true;
        }
        return false;
    }

    //Called by event
    public void Scope()
    {
        HUDManager.Instance.Scope();
        nightCamera.m_Lens.FieldOfView = 30;
        playerController.CurrentSensitivity = playerController.DesiredSensivity / 2;
    }

    //Called by event and eq
    public void UnScope()
    {
        nightCamera.m_Lens.FieldOfView = 90;
        HUDManager.Instance.UnScope();
        playerController.CurrentSensitivity = playerController.DesiredSensivity;
    }
}
