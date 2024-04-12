using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot
{
    public Building gridBuilding;
    public GameObject terrain;
    public Vector2Int slotPosition = new Vector2Int();
    private bool _isTaken = false;
    public bool IsTaken
    {
        get => _isTaken;
        set {_isTaken = value;}
    }
    public GridSlot()
    {
        gridBuilding = null;
        terrain = null;
        slotPosition = new Vector2Int();
        _isTaken = false;
    }
    public void SetGridSlot(Building building, Vector2Int slotPosition, bool isTaken)
    {
        this.gridBuilding = building;
        this.slotPosition = slotPosition;
        this._isTaken = isTaken;
    }
}
