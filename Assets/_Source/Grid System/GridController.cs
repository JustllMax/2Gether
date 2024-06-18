using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log($"{GetGridPos().x} {GetGridPos().y}");
        }
    }
    public bool TryPlace(Vector2Int pos, Building building)
    {
        if (!IsPlaceTaken(pos.x, pos.y))
        {
            GameObject newBuilding = Instantiate(building.gameObject, new Vector3(pos.x*10, 0, pos.y*10), building.gameObject.transform.rotation);
            _grid[math.abs(pos.x), math.abs(pos.y)].IsTaken = true;
            _grid[math.abs(pos.x), math.abs(pos.y)].gridBuilding = newBuilding.GetComponent<Building>();

            NavMeshSurfaceManager.Instance.BakeAllNavMeshes();
            return true;
        }
        return false;
    }
    public void SetGridSlot(Vector2Int position, GameObject terrain)
    {
        _grid[math.abs(position.x), math.abs(position.y)].slotPosition = position;
        _grid[math.abs(position.x), math.abs(position.y)].terrain = terrain;
    }
    public bool IsPlaceTaken(int x, int y)
    {
        if (_grid[math.abs(x), math.abs(y)].IsTaken)
        {
            return true;
        }
        return false;
    }

    public Vector2Int GetGridPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2Int rayPosition = new Vector2Int();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Terrain") || hit.collider.gameObject.CompareTag("Building"))
            {
                rayPosition.x = Mathf.RoundToInt(hit.point.x/10);
                rayPosition.y = Mathf.RoundToInt(hit.point.z/10);
            }
        }
        return rayPosition;
    }

    public Building GetBuilding(Vector2Int pos)
    {
        return _grid[math.abs(pos.x), math.abs(pos.y)].gridBuilding;
    }

    public void RemoveBuilding(Vector2Int pos)
    {
        if(_grid[math.abs(pos.x), math.abs(pos.y)].gridBuilding != null)
            Destroy(_grid[math.abs(pos.x), math.abs(pos.y)].gridBuilding.gameObject);
        _grid[math.abs(pos.x), math.abs(pos.y)] = new GridSlot();
    }
}
