using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private static GridController _instance;

    public static GridController Instance { get { return _instance; } }

    private GridSlot[,] _grid;

    public Vector2Int GridSize
    {
        get { return new Vector2Int(_grid.GetLength(0), _grid.GetLength(1)); }
    }

    public GridSlot[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }


    private bool IsPlaceTaken(int x, int y)
    {
        if (Grid[x, y] != null)
        {
            return true;
        }
        return false;
    }

    public bool TryPlace(Vector2Int pos, Building b)
    {
        var slot = Grid[pos.x, pos.y];

        bool _isAvailableToBuild = false;

        if (x < -5 || x > GridSize.x * 10 - slot.gridBuilding.BuildingSize.x)
            _isAvailableToBuild = false;
        else if (z < -5 || z > GridSize.y * 10 - slot.gridBuilding.BuildingSize.z)
            _isAvailableToBuild = false;
        else
            _isAvailableToBuild = true;

        if (_isAvailableToBuild && IsPlaceTaken((int)_draggingBuilding.transform.position.x / 10, (int)_draggingBuilding.transform.position.z / 10))
        {
            Debug.Log($"Drag X: {(int)_draggingBuilding.transform.position.x / 10}, Z:{(int)_draggingBuilding.transform.position.z / 10}");
            _isAvailableToBuild = false;
        }


        // actually place the thing
        return true;
    }
}
