using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : Gun
{

    bool isParryOnCD = false;
    float firingCooldown = 0;
    float firingCooldownTimer = 0;
    void Start()
    {
        ammoInMagazine = GetMagazineSize();
        firingCooldown = GetGunData().FireRate;
    }

    void Update()
    {
        if(firingCooldownTimer > 0)
        {
            firingCooldownTimer -= Time.deltaTime; 
        }
    }
    public override void Fire(bool isSameButtonPress)
    {
        if (firingCooldownTimer <= 0f)
        {
            if(isSameButtonPress)
            {
                return;
            }
            ammoInMagazine -= 1;
            firingCooldownTimer = firingCooldown;
            CalculateFire();
        }

    }

    private void CalculateFire()
    {
        Debug.Log(this + " Fire " + (ammoInMagazine - 1));
    }

    public override void Aim()
    {
        isAiming = true;

        Debug.Log(this + " Parry");

        isAiming = false;

    }


    public override bool CanAim()
    {
       if(isParryOnCD)
            return false;
       return true;
    }

    public override bool CanFire()
    {
        if(isAiming)
            return false;

        if(ammoInMagazine > 0)
            return true;

        return false;
    }


    
}
