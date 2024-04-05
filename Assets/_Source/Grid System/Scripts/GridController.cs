using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private static GridController _instance;

    public static GridController Instance { get { return _instance; } }

    [SerializeField] private Vector2Int _gridSize = new Vector2Int(12, 12);
    public Vector2Int gridSize { get => _gridSize; set {; } }
    private GridSlot[,] _grid;

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
        {
            _instance = this;
        }

        _grid = new GridSlot[_gridSize.x, _gridSize.y];
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                _grid[x, y] = new GridSlot();
            }
        }
    }
    public bool TryPlace(Vector2Int position, Building building)
    {
        if (!IsPlaceTaken(position.x, position.y))
        {
            GameObject newBuilding = Instantiate(building.gameObject, new Vector3(position.x, 0, position.y) * 10, Quaternion.identity);
            GridSlot gridSlot = new GridSlot(building, position, true);
            _grid[position.x, position.y] = gridSlot;
            return true;
        }
        return false;
    }

    public bool IsPlaceTaken(int x, int y)
    {
        if (_grid[x, y].IsTaken)
        {
            return true;
        }
        return false;
    }
}
