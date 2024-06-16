using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType : int
{
    Building,
    Player,
    MainBase,
    BuildingAll
}


public interface ITargetable
{
    public bool IsTargetable { get; set; }

    public TargetType TargetType { get; set; }

}
