using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Gun : MonoBehaviour
{
    [SerializeField] GunData _SO_Stats;
    [SerializeField] GameObject _Model;

    [SerializeField]protected AudioClip firingSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected bool isAiming;
    [SerializeField] protected int ammoInMagazine;

    public abstract void Fire(bool isSameButtonPress, Transform firePoint);
    public abstract void Aim();

    public abstract bool CanFire();

    public abstract bool CanAim();

    public int GetMagazineSize()
    {
        return _SO_Stats.MagazineSize;
    }

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

    public GameObject GetGunModel()
    {
        return _Model;
    }
}
