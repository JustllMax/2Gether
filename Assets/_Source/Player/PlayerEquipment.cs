using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerEquipment : MonoBehaviour
{
    PlayerInputAction.FPSControllerActions _FPSController;

    [SerializeField]List<Gun> GunList = new List<Gun>();
    [SerializeField] List<GunAmmoStore> AmmoStore;

    [SerializeField] AudioSource audioSource;
    Animator _animator;

    public Dictionary<GunType, int> AmmoStorage;
    public Gun _currentGun;
    public Gun _lastHeldGun;

    public int GrenadesLeft;

    private bool isSwitchingGun;
    private bool isReloading;
    private float reloadTimer;

    [Serializable]
    struct GunAmmoStore
    {
        public GunType type;
        public int amount;
    }

    private void Awake()
    {
        AmmoStorage = new Dictionary<GunType, int>();
        foreach(GunAmmoStore gun in AmmoStore) {
            AmmoStorage.Add(gun.type, gun.amount);
        }
        _animator = GetComponent<Animator>();
    }
    void Start()
    {

        _FPSController = InputManager.Instance.GetPlayerInputAction().FPSController;


        _FPSController.WeaponSwitch.performed += SwitchWeaponByHotkeys;
        _FPSController.SwitchToLastWeapon.performed += SwitchToLastHeldWeapon;
        Setup();
        
    }


    private void Update()
    {

        SwitchByScrolling();
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= _currentGun.GetGunData().ReloadTime)
            {

                ResetReloadTimer();
                if (!_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.RELOADUP.ToString()))
                {
                    _animator.CrossFade(PlayerAnimNames.RELOADUP.ToString(), 0.1f);

                }
            }
        }
    }

    #region SwitchGun

    public void SwitchWeaponByHotkeys(InputAction.CallbackContext context)
    {
        int index = (int)context.ReadValue<float>() - 1;

        if (GetGunIndexByRef(_currentGun) == index || index > GunList.Count - 1)
            return;

        SwitchCurrentGun(GunList[index]);

    }

    public void SwitchToLastHeldWeapon(InputAction.CallbackContext context)
    {
        if (_lastHeldGun == null) return;

        SwitchCurrentGun(_lastHeldGun);
    }

    public void SwitchByScrolling()
    {

        //TODO scroll is scrolling too fast
        //Also might have to change to press to confirm weapon selection, not instantly changing

        int input = (int)Mathf.Clamp(_FPSController.Scroll.ReadValue<float>(), -1, 1);

        if (input == 0)
            return;

        Debug.Log(this + " " + input);


        int currentGunIndex = GetGunIndexByRef(_currentGun);
        int indexToSwitchTo = currentGunIndex;
        indexToSwitchTo += input;


        if (indexToSwitchTo >= GunList.Count)
        {
            indexToSwitchTo = 0;
        }
        else if(indexToSwitchTo < 0) 
        {
            indexToSwitchTo = GunList.Count-1;
        }
        
        if(indexToSwitchTo != currentGunIndex)
        {
            
            SwitchCurrentGun(GunList[indexToSwitchTo]);
        }
            
    }





    private void SwitchCurrentGun(Gun gun)
    {
        ResetReloadTimer();
        isSwitchingGun = true;


        if ( !_animator.GetNextAnimatorStateInfo(0).IsName(PlayerAnimNames.SWITCHDOWN.ToString()))
        {
            _animator.CrossFade(PlayerAnimNames.SWITCHDOWN.ToString(), 0.1f);
        }

        GetAudioSource().Stop();

        _lastHeldGun = _currentGun;
        _currentGun = gun;

        HUDManager.Instance.SwitchGunOnHUD(_currentGun.GetAmmoInMagazine(), _currentGun.GetGunData().MagazineSize, 
            AmmoStorage[_currentGun.GetGunData().GunType], _currentGun.GetGunData().GunType);
    }

    public void SwitchDownEndAnimEvent()
    {

        Debug.Log(this + " anim down");
        foreach (Gun gun in GunList)
        {
            gun.GetGunModel().SetActive(false);
        }
        _currentGun.GetGunModel().SetActive(true); 
    }
    public void SwitchUpEndAnimEvent()
    {
        isSwitchingGun = false;
        Debug.Log(this + " anim up");


    }
    #endregion SwitchGun


    #region Reload

    public void ReloadDownStartAnimEvent()
    {
        isReloading = true;
        AudioManager.Instance.PlaySFXAtSource(_currentGun.GetReloadSFX(), GetAudioSource());

    }

    public void ReloadUpStartAnimEvent()
    {
        ResetReloadTimer();
        Reload();
    }

    //Called by AnimEvents only
    private bool Reload()
    {
        GunType gunType = _currentGun.GetGunData().GunType;
        int magSize = _currentGun.GetGunData().MagazineSize;
        int currentAmmo = _currentGun.GetAmmoInMagazine();
        int amountToReload = magSize - currentAmmo;

        switch (gunType)
        {
            case GunType.Pistol:
                _currentGun.SetAmmoInMagazine(magSize);
                HUDManager.Instance.SetCurrentAmmo(_currentGun.GetAmmoInMagazine());
                return true;

            default:

                if (AmmoStorage[gunType] > 0)
                {
                    if (amountToReload > AmmoStorage[gunType])
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + AmmoStorage[gunType]);
                        AmmoStorage[gunType] = 0;
                        HUDManager.Instance.SetCurrentAmmo(_currentGun.GetAmmoInMagazine(), AmmoStorage[gunType]);

                        return true;
                    }
                    else
                    {
                        _currentGun.SetAmmoInMagazine(currentAmmo + amountToReload);
                        AmmoStorage[gunType] -= amountToReload;
                        HUDManager.Instance.SetCurrentAmmo(_currentGun.GetAmmoInMagazine(), AmmoStorage[gunType]);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
        }

    }

    private void ResetReloadTimer()
    {
        isReloading = false;
        reloadTimer = 0f;
    }

    #endregion Reload

    #region Utils

    void Setup()
    {
        SwitchCurrentGun(GunList[0]);

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

    #endregion Utils


    #region GetSet

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
        int currentAmmo = _currentGun.GetAmmoInMagazine();
        int magSize = _currentGun.GetGunData().MagazineSize;
        //ammo in magazine more than max || AmmoStorage is 0
        if (currentAmmo >= magSize || AmmoStorage[_currentGun.GetGunData().GunType] == 0)
            return false;

        return true;

    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    #endregion GetSet
    

}
