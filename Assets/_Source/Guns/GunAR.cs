  using System.Collections;
using System.Linq;
using UnityEngine;

public class GunAR : Gun
{
    [SerializeField] PlayerEquipment playerEQ;

    [Header("Grenade Launcher")]
    [SerializeField] GameObject P_PlayerGrenade;
    [SerializeField] float granadeSpeed;
    [SerializeField] float explosionDamage;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask explosionMask;


    
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

        ShootGrenadeLauncher();
        playerEQ.GrenadesLeft--;
        isAiming = false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
        Debug.DrawRay(trailSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
    }
    public override bool CanAim()
    {
        if(playerEQ.GrenadesLeft > 0)
        {
            return true;
        }    
       return false;
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


    void ShootGrenadeLauncher()
    {

        RaycastHit hit;
        Vector3 direction = bulletSpawnPoint.forward;
        Vector3 endPoint = Vector3.zero;
        
        if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, GetGunData().Range, mask))
        {
            endPoint = hit.point;
            direction = (endPoint - trailSpawnPoint.position).normalized;
        }
        else{

            endPoint = bulletSpawnPoint.position + direction * 100;
            direction = (endPoint - trailSpawnPoint.position).normalized;
        }
        PlayerGrenade grenade = Instantiate(P_PlayerGrenade, trailSpawnPoint.position, Quaternion.identity).GetComponent<PlayerGrenade>();
        grenade.SetUp(granadeSpeed, direction, 12.0f, explosionDamage, explosionRadius, explosionMask);
    }

}
