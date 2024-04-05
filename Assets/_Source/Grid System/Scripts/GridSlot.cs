using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot
{
    public Building gridBuilding;
    public Vector2Int slotPosition = new Vector2Int();
    private bool _isTaken = false;
    public bool IsTaken
    {
        get => _isTaken;
        set {_isTaken = value;}
    }

    public GridSlot(Building building, Vector2Int slotPosition, bool isTaken)
    {
        this.gridBuilding = building;
        this.slotPosition = slotPosition;
        this._isTaken = isTaken;
    }
    public GridSlot()
    {
        this.slotPosition = new Vector2Int();
        this._isTaken = false;
    }
}
