using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildCardManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    #region Cards
    private GridCard _cardSO;
    public GridCard CardSO
    {
        get => _cardSO;
        set { _cardSO = value; }
    }
    #endregion

    #region Buildings
    private GameObject _draggingBuilding;
    private GridSlot _gridSlot;
    #endregion

    #region Grid
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(12, 12);
    private GridController _gridController;
    private bool _isAvailableToBuild;
    private float y;
    #endregion

    private void Awake()
    {
        _gridController = GridController.Instance;
        _gridController.Grid = new GridSlot[_gridSize.x, _gridSize.y];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggingBuilding = Instantiate(_cardSO.prefab, Vector3.zero, Quaternion.identity);
        _gridSlot = new GridSlot
        {
            gridBuilding = _draggingBuilding.GetComponent<GridBuilding>()
        };

        y = _gridSlot.gridBuilding.BuildingSize.y;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Terrain"))
            {
                GameObject terrain = hit.collider.gameObject;
                int x = Mathf.RoundToInt(hit.point.x);
                int z = Mathf.RoundToInt(hit.point.z);

                _draggingBuilding.transform.position = new Vector3(x, y, z);
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggingBuilding != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject terrain = hit.collider.gameObject;
                    int x = Mathf.RoundToInt(hit.point.x);
                    int z = Mathf.RoundToInt(hit.point.z);

                    #region Check Is available to build

                    if (x < -5 || x > _gridSize.x * 10 - _gridSlot.gridBuilding.BuildingSize.x)
                        _isAvailableToBuild = false;
                    else if (z < -5 || z > _gridSize.y * 10 - _gridSlot.gridBuilding.BuildingSize.z)
                        _isAvailableToBuild = false;
                    else
                        _isAvailableToBuild = true;

                    if (_isAvailableToBuild && IsPlaceTaken((int)_draggingBuilding.transform.position.x / 10, (int)_draggingBuilding.transform.position.z / 10))
                    {
                        Debug.Log($"Drag X: {(int)_draggingBuilding.transform.position.x / 10}, Z:{(int)_draggingBuilding.transform.position.z / 10}");
                        _isAvailableToBuild = false;
                    }
                    #endregion

                    _draggingBuilding.transform.position = new Vector3(terrain.gameObject.transform.position.x, y, terrain.gameObject.transform.position.z);
                    _gridSlot.gridBuilding.SetColor(_isAvailableToBuild);
                }

            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isAvailableToBuild)
            Destroy(_draggingBuilding);
        else
        {
            _gridController.Grid[(int)_draggingBuilding.transform.position.x / 10, (int)_draggingBuilding.transform.position.z / 10] = _gridSlot;
            Debug.Log($"Point up X: {(int)_draggingBuilding.transform.position.x / 10}, Z:{(int)_draggingBuilding.transform.position.z / 10}");
            _gridSlot.gridBuilding.ResetColor();
        }
    }

    private bool IsPlaceTaken(int x, int y)
    {
        if (_gridController.Grid[x, y] != null)
        {
            return true;
        }
        return false;
    }
}