using System.Collections;
using UnityEngine;

public class GunPistol : Gun
{

    bool isParryOnCD = false;



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

    private Animator animator;
    private float LastShootTime;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        ammoInMagazine = GetMagazineSize();
        shootDelay = GetGunData().FireRate;
        LastShootTime = Time.time;
    }

    public override bool Fire(bool isSameButtonPress, Transform bulletSpawnPoint)
    {
        if (LastShootTime + shootDelay < Time.time)
        {
            if(isSameButtonPress)
            {
                return false;
            }
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
        
        Debug.Log(this + " Parry");

        isAiming = false;
        return true;
    }

    private void OnDrawGizmos()
    {

        Debug.DrawRay(bulletSpawnPoint.position, bulletSpawnPoint.forward * GetGunData().Range, Color.green);
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
        if(!audioSource.isPlaying)
            AudioManager.Instance.PlaySFXAtSource(noAmmoSound, audioSource);
        GameManager.Instance.GetPlayerController().GetComponent<PlayerGunController>().ReloadWeapon();
        return false;
    }


    //TO DO: FIX ANGLE OF TRAIL TO MATCH AIM POSITION
    private void CalculateFire(Transform bulletSpawnPoint)
    {

        RaycastHit hit;


        Vector3 direction = GetDirection();

        if (Physics.Raycast(bulletSpawnPoint.position, direction, out  hit, GetGunData().Range, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
            IDamagable target = null;
            if(hit.transform.TryGetComponent<AIController>(out AIController controller))
            {
                target = controller.GetComponent<IDamagable>();
            }
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, target));

            LastShootTime = Time.time;
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position + GetDirection() * 100, Vector3.zero, false, null));

            LastShootTime = Time.time;
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

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, IDamagable target)
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
            if(target != null)
                target.TakeDamage(GetGunData().BulletDamage);
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
