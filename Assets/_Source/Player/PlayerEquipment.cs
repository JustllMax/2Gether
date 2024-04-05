using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerEquipment : MonoBehaviour
{
    PlayerInputAction.FPSControllerActions _FPSController;

    List<Gun> GunList = new List<Gun>();

    public Gun _currentGun;
    public Gun _lastHeldGun;

    public float ARAmmo = 60;
    public float ShotgunAmmo;
    public float SniperAmmo;
    public float GrenadesLeft;

    private bool isSwitchingGun;
    private bool isReloading;
    private bool isFiring;

    void Start()
    {
        _FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;

        _FPSController.WeaponSwitch.performed += SwitchWeaponByHotkeys;
        _FPSController.SwitchToLastWeapon.performed += SwitchToLastHeldWeapon;
    }


    public void SwitchWeaponByHotkeys(InputAction.CallbackContext context)
    {
        isSwitchingGun = true;
        
        //PlayAnimation

        int index = (int)context.ReadValue<float>() - 1;
        if(index > GunList.Count-1) {
            return;
        }

        _lastHeldGun = _currentGun;

        _currentGun = GunList[index];

        isSwitchingGun = false;
    }

    public void SwitchToLastHeldWeapon(InputAction.CallbackContext context)
    {

        isSwitchingGun = true;


        //PlayAnimation

        var temp = _currentGun;

        _currentGun = _lastHeldGun;

        _lastHeldGun = temp;

        isSwitchingGun = false;

    }

    private int GetGunIndexByRef(Gun gun)
    {
        for(int i = 0; i < GunList.Count; i++ )
        {
            if (gun.Equals(GunList[i]))
            {
                return i;
            }
        }
        return -1;
    }

    public Gun GetCurrentGun()
    {
        return _currentGun;
    }

    public GunData GetGunData()
    {
        return _currentGun.GetGunData();
    }

    public bool CanFire()
    {
        if(_currentGun == null)
            return false;

        if(isSwitchingGun || isReloading || isFiring)
            return false;


        return _currentGun.CanFire();
    }

    public bool CanAim()
    {
        if (_currentGun == null)
            return false;

        if (isSwitchingGun || isReloading)
            return false;

        return _currentGun.CanAim();
    }

    public bool CanReload()
    {
        if (_currentGun == null)
            return false;

        if (isSwitchingGun)
            return false;


        return _currentGun.CanReload();
    }
}
