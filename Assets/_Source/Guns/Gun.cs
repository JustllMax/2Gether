using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Gun : MonoBehaviour
{
    [SerializeField] GunData _SO_Stats;

    protected int ammoInMagazine;

    protected AudioClip firingSound;
    protected AudioClip reloadSound;

    public abstract void Fire();
    public abstract void Aim();
    public abstract void Reload();

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


    public GunData GetGunData()
    {
        return _SO_Stats;
    }
}
