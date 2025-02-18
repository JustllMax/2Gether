using System.Collections;
using UnityEngine;

abstract public class Gun : MonoBehaviour
{
    [SerializeField] GunData _SO_Stats;
    [SerializeField] GameObject _Model;


    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip firingSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected AudioClip noAmmoSound;
    [SerializeField] protected int ammoInMagazine;
    [SerializeField] protected float bulletSpeed = 100;
    [SerializeField] protected bool addBulletSpread = true;
    [SerializeField] protected Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] protected MuzzleFlashSpawner muzzleFlash;
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] protected Transform trailSpawnPoint;
    [SerializeField] protected ParticleSystem impactParticleSystem;
    [SerializeField] protected TrailRenderer bulletTrail;
    [SerializeField] protected LayerMask mask;
    [SerializeField] protected LayerMask bulletHoleMask;
    [SerializeField] protected WeaponIKConfig weaponIKConfig;
    protected float shootDelay;
    protected float lastShootTime;
    protected Animator animator;
    public bool isAiming;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();
        ammoInMagazine = GetMagazineSize();
        shootDelay = GetGunData().FireRate;
        lastShootTime = Time.time;
    }

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

    protected IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, bool makeBulletHole, IDamagable target, float damageModifier)
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
            if (makeBulletHole)
            {
                Instantiate(impactParticleSystem, HitPoint + HitNormal * 0.01f, Quaternion.LookRotation(HitNormal));
            }
                
            if (target != null)
                target.TakeDamage(GetGunData().BulletDamage * damageModifier);
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    protected Vector3 GetDirection()
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

    protected virtual void CalculateFire(Transform bulletSpawnPoint)
    {

        RaycastHit hit;
        Vector3 direction = GetDirection();

        if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, GetGunData().Range, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
            IDamagable target = null;
            float damageModifier = 1.0f;
            if (hit.transform.TryGetComponent(out AIController controller))
            {
                target = controller.GetComponent<IDamagable>();

                if (hit.collider.TryGetComponent(out ColliderDamageModifier modifier))
                {
                    damageModifier = modifier.damageModifier;
                }
            }
            bool leaveBulletHole = (hit.collider.gameObject.layer & bulletHoleMask.value) > 0;
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, leaveBulletHole, target, damageModifier));

            lastShootTime = Time.time;
        }
        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position + GetDirection() * 100, Vector3.zero, false, false, null, 1));

            lastShootTime = Time.time;
        }

    }

    public void StopSFX()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    public WeaponIKConfig GetIKConfig()
    {
        return weaponIKConfig;
    }
    public virtual void SetIsAiming(bool val)
    {
        isAiming = val;
    }

}
