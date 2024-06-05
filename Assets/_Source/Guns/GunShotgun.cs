using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotgun : Gun
{
    [SerializeField]
    private bool addBulletSpread = true;
    [SerializeField]
    private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem shootingSystem;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Transform trailSpawnPoint;
    [SerializeField]
    private ParticleSystem impactParticleSystem;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float bulletSpeed = 100;

    private float shootDelay;
    private float spreadAngle = 2f; // K¹t rozproszenia dla dwóch pocisków
    private int pelletsPerShot = 6;

    private Animator animator;
    private float lastShootTime;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        ammoInMagazine = GetMagazineSize();
        shootDelay = GetGunData().FireRate;
    }

    public override void Fire(bool isSameButtonPress, Transform bulletSpawnPoint)
    {
        if (lastShootTime + shootDelay < Time.time)
        {
            if (isSameButtonPress)
            {
                return;
            }
            ammoInMagazine -= 1;
            CalculateFire(bulletSpawnPoint, pelletsPerShot);
        }
    }

    public override void Aim()
    {
        isAiming = true;

        Debug.Log(this + " Aim");

        if (lastShootTime + shootDelay < Time.time)
        {
            ammoInMagazine -= 2;

            lastShootTime = Time.time;
        }

        isAiming = false;
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

        return false;
    }

    private void CalculateFire(Transform bulletSpawnPoint, int pelletCount)
    {
        Debug.Log(this + " Fire " + (ammoInMagazine - 1));

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 direction = GetDirection();
            RaycastHit hit;

            if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, GetGunData().Range, mask))
            {
                TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position + direction * 100, Vector3.zero, false));
            }
        }

        lastShootTime = Time.time;
    }

    private void CalculateFire(Transform bulletSpawnPoint, Vector3 direction, int pelletCount)
    {
        for (int i = 0; i < pelletCount; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, GetGunData().Range, mask))
            {
                TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position + direction * 100, Vector3.zero, false));
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = bulletSpawnPoint.forward;

        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            Instantiate(impactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
