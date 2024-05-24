using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : Gun
{

    bool isParryOnCD = false;
    [SerializeField]
    float firingCooldown = 0.5f;
    [SerializeField]
    float firingCooldownTimer = 0f;
    [SerializeField] Transform firePoint;
    void Start()
    {
        ammoInMagazine = GetMagazineSize();
        firingCooldown = GetGunData().FireRate;
    }

    void Update()
    {
        if(firingCooldownTimer > 0.1f)
        {
            firingCooldownTimer -= Time.deltaTime; 
        }
    }
    public override void Fire(bool isSameButtonPress, Transform firePoint)
    {
        Debug.Log("Fire attempt");
        if (firingCooldownTimer <= 0f)
        {
            if(isSameButtonPress)
            {
                return;
            }
            ammoInMagazine -= 1;
            firingCooldownTimer = firingCooldown;
            CalculateFire(firePoint);
        }

    }

    private void CalculateFire(Transform firePoint)
    {
        Debug.Log(this + " Fire " + (ammoInMagazine - 1));

        RaycastHit hit;

        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, GetGunData().Range))
        {
            
            Debug.Log( hit.transform.gameObject + " Hit!");
            hit.transform.GetComponent<Renderer>().enabled = false;
        }

    }

    public override void Aim()
    {
        isAiming = true;

        Debug.Log(this + " Parry");

        isAiming = false;

    }

    private void OnDrawGizmos()
    {

        Debug.DrawRay(firePoint.position, firePoint.forward * GetGunData().Range, Color.green);
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
