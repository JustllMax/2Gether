using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Building,
    Player,
    Both // ONLY FOR AI TO PICK
}


public interface ITargetable
{
    public bool IsTargetable { get; set; }

    public TargetType TargetType { get; set; }

}
