using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{

    [SerializeField] Transform FirePoint;
    PlayerInputAction.FPSControllerActions _FPScontroller;
    PlayerEquipment _equipment;

    bool firedButtonHeld = false;
    float buttonHeldTimer = 0;
    bool isHoldingFire = false;


    private void Awake()
    {
        _equipment = GetComponent<PlayerEquipment>();
    }

    void Start()
    {

        _FPScontroller = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPScontroller.Fire.performed += PerformFireWeapon;
        _FPScontroller.Fire.canceled += CancelFireWeapon;
        _FPScontroller.Aim.performed += AimWeapon;
        _FPScontroller.Reload.performed += ReloadWeapon;

    }

    private void Update()
    {
        if(firedButtonHeld)
        {
            buttonHeldTimer += Time.deltaTime;
            if(buttonHeldTimer > 0.01)
            {
                isHoldingFire = true;
            }
            if (_equipment != null)
            {
                if (_equipment.CanFire())
                {
                    _equipment.GetCurrentGun().Fire(isHoldingFire, FirePoint);
                }

            }
        }
    }

    private void PerformFireWeapon(InputAction.CallbackContext context)
    {
        if (context.performed) // the key has been pressed
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
        }
    }

    private void AimWeapon(InputAction.CallbackContext context)
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
            if (_equipment.CanReload())
            {
                _equipment.Reload();
            }
        }
        
    }
}
