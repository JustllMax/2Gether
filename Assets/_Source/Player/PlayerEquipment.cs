using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    PlayerInputAction.FPSControllerActions _FPSController;

    List<int> GunList = new List<int>();

    public int _currentGun;
    public int _lastHeldGun;
    void Start()
    {
        _FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPSController.WeaponSwitch.performed += SwitchWeaponByHotkeys;
        _FPSController.SwitchToLastWeapon.performed += SwitchToLastHeldWeapon;
    }


    public void SwitchWeaponByHotkeys(InputAction.CallbackContext context)
    {

        float index = context.ReadValue<float>() - 1;
        if(index > GunList.Count-1) {
            return;
        }

        _lastHeldGun = _currentGun;

        //_currentGun = GunList[index];

    }

    public void SwitchToLastHeldWeapon(InputAction.CallbackContext context)
    {
        Debug.Log(this + " Q " +  context.ReadValueAsButton().ToString());
    }

    private void GetGunByType()
    {
        
    }

    public int GetCurrentGun()
    {
        return _currentGun;
    }

    public bool CanFire()
    {
        return true;
    }

    public bool CanAim()
    {
        return true;
    }

    public bool CanReload()
    {
        return true;
    }
}
