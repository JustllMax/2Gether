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
            ammoInMagazine -= 2;
            CalculateFire(bulletSpawnPoint);
            CalculateFire(bulletSpawnPoint);
            lastShootTime = Time.time;

            HUDManager.Instance.SetCurrentAmmo(ammoInMagazine);
            GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon();

            isAiming = false;
            return true;
        }

        isAiming = false;
        return false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
    }

    public override bool CanAim()
    {
        if (isAiming)
            return false;

        if (ammoInMagazine > 1)
            return true;

        return false;
    }

    public override bool CanFire()
    {
        if (isAiming)
            return false;

        if (ammoInMagazine > 0)
            return true;
  
        GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon();
        return false;
    }

    protected override void CalculateFire(Transform bulletSpawnPoint)
    {
        AudioManager.Instance.PlaySFXAtSource(firingSound, audioSource);
        Debug.Log(this + " Fire " + (ammoInMagazine - 1));

        for (int i = 0; i < pelletsPerShot; i++)
        {
            base.CalculateFire(bulletSpawnPoint);
        }
    }
}
