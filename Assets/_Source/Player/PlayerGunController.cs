using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunController : MonoBehaviour
{

    PlayerInputAction.FPSControllerActions _FPScontroller;
    PlayerEquipment _equipment;

    private void Awake()
    {
        _equipment = GetComponent<PlayerEquipment>();
    }

    void Start()
    {

        _FPScontroller = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPScontroller.Fire.performed += FireWeapon;
        _FPScontroller.Aim.performed += AimWeapon;
        _FPScontroller.Reload.performed += ReloadWeapon;

    }


    public void FireWeapon(InputAction.CallbackContext context)
    {

        bool isSameButton = false;
        //TODO: check probly with context.time start time if same button press holding

        if(_equipment != null)
        {
            if (_equipment.CanFire())
            {
                _equipment.GetCurrentGun().Fire(isSameButton);
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
                _equipment.Reload();
            }
        }
        
    }
}
