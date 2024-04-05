using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{

    PlayerInputAction.FPSControllerActions _FPScontroller;
    PlayerEquipment _equipment;
    void Start()
    {

        _equipment = GetComponent<PlayerEquipment>();
        _FPScontroller = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPScontroller.Fire.performed += FireWeapon;
        _FPScontroller.Aim.performed += AimWeapon;
        _FPScontroller.Reload.performed += ReloadWeapon;

    }


    public void FireWeapon(InputAction.CallbackContext context)
    {
        
        if(_equipment != null)
        {
            if (_equipment.CanFire())
            {
                _equipment.GetCurrentGun().Fire();
            }
        }
        
    }

    public void AimWeapon(InputAction.CallbackContext context)
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
                _equipment.GetCurrentGun().Reload();
            }
        }
        
    }
}
