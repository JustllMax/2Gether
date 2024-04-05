using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Pistol,
    AR,
    Shotgun,
    Sniper
}

public abstract class GunData : CardStatistics
{
    public GunType GunType;
    public GameObject P_Bullet;
    public AudioClip BulletGroundHitSound;
    public AudioClip BulletEnemyHitSound;
    public bool IsAutomatic;
    public int MagazineSize;
    public int FireRate;
    public float Range;
    public float ReloadTime;
    public float BulletDamage;


}
