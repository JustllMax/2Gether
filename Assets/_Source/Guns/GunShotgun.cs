using System.Collections;
using System.Linq;
using UnityEngine;

public class GunShotgun : Gun
{
    [SerializeField]
    private int pelletsPerShot = 6;

    public override bool Fire(bool isSameButtonPress, Transform bulletSpawnPoint)
    {
        if (lastShootTime + shootDelay < Time.time)
        {
            if (isSameButtonPress)
            {
                return false;
            }

            ammoInMagazine -= 1;
            CalculateFire(bulletSpawnPoint);
            return true;
        }
        return false;
    }

    public override bool Aim()
    {
        isAiming = true;

        Debug.Log(this + " Aim");

        if (lastShootTime + shootDelay < Time.time)
        {
            int maxAmmo = ammoInMagazine;
            for(int i = 0; i<maxAmmo; i++)
            {
                Debug.Log("Shotgun shoot " + ammoInMagazine);
                ammoInMagazine--;
                CalculateFire(bulletSpawnPoint);
            }
            lastShootTime = Time.time;

            HUDManager.Instance.SetCurrentAmmo(ammoInMagazine);
            GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon();

            isAiming = false;
            return true;
        }

        isAiming = false;
        return false;
    }


    public override bool CanAim()
    {
        return TryShooting();
    }

    public override bool CanFire()
    {
        return TryShooting();
    } 

    bool TryShooting()
    {
        if (isAiming)
            return false;

        if (ammoInMagazine > 0)
            return true;

        if (GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon() == false)
        {
            AudioManager.Instance.PlaySFXAtSourceOnce(GetNoAmmoSFX(), GetAudioSource());
        }
        return false;
    }

    protected override void CalculateFire(Transform bulletSpawnPoint)
    {
        AudioManager.Instance.PlaySFXAtSource(firingSound, audioSource);


        for (int i = 0; i < pelletsPerShot; i++)
        {
            base.CalculateFire(bulletSpawnPoint);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
    }
}
