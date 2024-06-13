using System.Collections;
using System.Linq;
using UnityEngine;

public class GunAR : Gun
{


    public override bool Fire(bool isSameButtonPress, Transform bulletSpawnPoint)
    {
        if (lastShootTime + shootDelay < Time.time)
        {
            ammoInMagazine -= 1;
            CalculateFire(bulletSpawnPoint);
            AudioManager.Instance.PlaySFXAtSource(firingSound, audioSource);
            shootingSystem.Play();
            return true;
        }
        return false;
    }


    public override bool Aim()
    {
        isAiming = true;
        


        isAiming = false;
        return true;
    }

    private void OnDrawGizmos()
    {

        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
    }
    public override bool CanAim()
    {
       return true;
    }

    public override bool CanFire()
    {
        if(isAiming)
            return false;

        if(ammoInMagazine > 0)
            return true;
        if(!audioSource.isPlaying)
            AudioManager.Instance.PlaySFXAtSource(noAmmoSound, audioSource);
        GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon();
        return false;
    }
}
