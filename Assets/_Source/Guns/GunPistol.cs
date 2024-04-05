using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : Gun
{

    bool isParryOnCD = false;

    public override void Fire(bool isSameButtonPress)
    {
        if(isSameButtonPress)
        {
            return;
        }
        Debug.Log(this + " Fire");
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


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
