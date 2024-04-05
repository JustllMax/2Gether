using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerEquipment : MonoBehaviour
{
    PlayerInputAction.FPSControllerActions _FPSController;

    [SerializeField]List<Gun> GunList = new List<Gun>();

    public Gun _currentGun;
    public Gun _lastHeldGun;

    public int ARAmmo = 60;
    public int ShotgunAmmo;
    public int SniperAmmo;
    public int GrenadesLeft;

    private bool isSwitchingGun;
    private bool isReloading;
    private bool isFiring;

    void Start()
    {
        _FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;


        _FPSController.WeaponSwitch.performed += SwitchWeaponByHotkeys;
        _FPSController.SwitchToLastWeapon.performed += SwitchToLastHeldWeapon;
    }


    private void Update()
    {
        SwitchByScrolling();
    }

    public void SwitchWeaponByHotkeys(InputAction.CallbackContext context)
    {
        // if (isSwitchingGun) return;
        int index = (int)context.ReadValue<float>() - 1;

        if (GetGunIndexByRef(_currentGun) == index)
            return;

        

        if(index > GunList.Count-1) {
            return;
        }

        SwitchCurrentGun(GunList[index]);

    }

    public void SwitchToLastHeldWeapon(InputAction.CallbackContext context)
    {
        // if (isSwitchingGun) return;

        if (_lastHeldGun == null) return;

        SwitchCurrentGun(_lastHeldGun);


    }

    public void SwitchByScrolling()
    {
        // if (isSwitchingGun) return;


        //TODO scroll is scrolling too fast
        //Also might have to change to press to confirm weapon selection, not instantly changing

        int input = (int)Mathf.Clamp(_FPSController.Scroll.ReadValue<float>(), -1, 1);

        if (input == 0)
            return;

        Debug.Log(this + " " + input);


        int currentGunIndex = GetGunIndexByRef(_currentGun);
        int indexToSwitchTo = currentGunIndex;
        indexToSwitchTo += input;

        if(indexToSwitchTo >= GunList.Count)
        {
            indexToSwitchTo = 0;
        }
        else if(indexToSwitchTo < 0) 
        {
            indexToSwitchTo = 0;
        }
        
        if(indexToSwitchTo != currentGunIndex)
        {
            
            SwitchCurrentGun(GunList[indexToSwitchTo]);
        }
            

    }


    private void SwitchCurrentGun(Gun gun)
    {
        isSwitchingGun = true;

        //PlayAnimation
        //Show on UI
        //Fire event that those 2 can subscribe to
        _lastHeldGun = _currentGun;
        _currentGun = gun;
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

        if(isSwitchingGun || isReloading)
            return false;


        return _currentGun.CanFire();
    }

    public bool CanAim()
    {
        if (_currentGun == null)
            return false;

        if (isSwitchingGun)
            return false;

        return _currentGun.CanAim();
    }

    public bool CanReload()
    {
        if (_currentGun == null)
            return false;

        if (isSwitchingGun)
            return false;

        if (_currentGun.GetAmmoInMagazine() >= _currentGun.GetGunData().MagazineSize)
            return false;

        return true;

    }

    public bool Reload()
    {

        int magSize = _currentGun.GetGunData().MagazineSize;
        int currentAmmo = _currentGun.GetAmmoInMagazine();
        int amountToReload = magSize - currentAmmo;


        switch (_currentGun.GetGunData().GunType) {
            case GunType.Pistol:
                _currentGun.SetAmmoInMagazine(magSize);
                return true;

            case GunType.AR:
                if(ARAmmo > 0)
                {
                    if(amountToReload > ARAmmo)
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + ARAmmo);
                        ARAmmo -= ARAmmo;
                        return true;
                    }
                    else
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + amountToReload);
                        ARAmmo -= amountToReload;
                        return true;

                    }
                }
                else
                {
                    return false;
                }
            case GunType.Shotgun:
                if (ShotgunAmmo > 0)
                {
                    if (amountToReload > ShotgunAmmo)
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + ShotgunAmmo);
                        ShotgunAmmo -= ShotgunAmmo;
                        return true;
                    }
                    else
                    {
                        _currentGun.SetAmmoInMagazine( currentAmmo + amountToReload);
                        ShotgunAmmo -= amountToReload;
                        return true;

                    }
                }
                else
                {
                    return false;
                }

            case GunType.Sniper:
                if (SniperAmmo > 0)
                {
                    if (amountToReload > SniperAmmo)
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + SniperAmmo);
                        SniperAmmo -= SniperAmmo;
                        return true;
                    }
                    else
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + amountToReload);
                        SniperAmmo -= amountToReload;
                        return true;

                    }
                }
                else
                {
                    return false;
                }
        }

        return false;
    }

}
