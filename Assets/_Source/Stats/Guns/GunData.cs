using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunData : CardStatistics
{

    public GameObject P_Bullet;
    public AudioClip BulletGroundHitSound;
    public AudioClip BulletEnemyHitSound;
    public int MagazineSize;
    public int FireRate;
    public float Range;
    public float ReloadTime;
    public float BulletDamage;


}
