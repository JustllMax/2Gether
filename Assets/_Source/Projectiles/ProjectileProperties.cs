using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_", menuName = "2Gether/Projectile")]
public class ProjectileProperties : ScriptableObject
{
    public float speed;
    public float damage;
    public float lifetime;
    public LayerMask surfaceMask;
    public LayerMask damageMask;
    public Material material;
    public bool hasGravity;

    public bool explodes;
    public float explosionRadius;
}
