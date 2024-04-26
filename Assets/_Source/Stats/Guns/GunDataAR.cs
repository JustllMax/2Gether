using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AR", menuName ="2Gether/Guns/Data/AR")]
public class GunDataAR : GunData
{
    public GameObject P_ARGrenade;
    public AudioClip GrenadeExplosionSound;
    public float GrenadeReloadTimer;
    public float GrenadeDamage;

}
