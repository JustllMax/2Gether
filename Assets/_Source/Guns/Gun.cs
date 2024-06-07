using UnityEngine;

abstract public class Gun : MonoBehaviour
{
    [SerializeField] GunData _SO_Stats;
    [SerializeField] GameObject _Model;


    [SerializeField]protected AudioSource audioSource;
    [SerializeField]protected AudioClip firingSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected AudioClip noAmmoSound;
    [SerializeField] protected bool isAiming;
    [SerializeField] protected int ammoInMagazine;

    public abstract bool Fire(bool isSameButtonPress, Transform bulletSpawnPoint);
    public abstract bool Aim();

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

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public AudioClip  GetReloadSFX()
    {
        return reloadSound;
    }

    public AudioClip GetNoAmmoSFX()
    {
        return noAmmoSound;
    }

}
