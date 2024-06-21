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
    public bool IsAutomatic;
    public int MagazineSize;
    public float FireRate;
    public float Range;
    public float ReloadTime;
    public float BulletDamage;

    public float amountToRestore;

    public override List<(string, string)> GetStatistics()
    {
        var s = base.GetStatistics();
        if(amountToRestore > 0)
        {
            addStat("Amount", amountToRestore.ToString());
        }
        return collectStat();
    }

}
