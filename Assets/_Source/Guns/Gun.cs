using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Gun : MonoBehaviour
{
    [SerializeField] GunData _SO_Stats;


    protected AudioClip firingSound;
    protected AudioClip reloadSound;
    protected bool isAiming;

    protected int ammoInMagazine;

    public abstract void Fire(bool isSameButtonPress);
    public abstract void Aim();

    public abstract bool CanFire();

    public abstract bool CanAim();
    public int GetAmmoInMagazine()
    {
        return ammoInMagazine;
    }

    public void SetAmmoInMagazine(int value)
    {
        ammoInMagazine = value;
        return;
    }

    public GunData GetGunData()
    {
        return _SO_Stats;
    }
}
