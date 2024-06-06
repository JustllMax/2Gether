using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    private float y;
    private GameObject _terrain;
    #endregion

    #region Grid
    private GridController _gridController;
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(12, 12);
    private bool _isAvailableToBuild;
    [SerializeField] private const int gridOffset = 10;
    #endregion

    private Vector2Int rayPosition;
    private void Awake()
    {
        _gridController = GridController.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggingBuilding = Instantiate(_cardSO.prefab, Vector3.zero, Quaternion.identity);
        y = _draggingBuilding.transform.localScale.y / 2;

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
                    _terrain = hit.collider.gameObject;
                    rayPosition.x = Mathf.RoundToInt(math.abs(hit.point.x));
                    rayPosition.y = Mathf.RoundToInt(math.abs(hit.point.z));
                    if (_draggingBuilding.GetComponent<GridBuilding>().IsDecorationCollision)
                        _isAvailableToBuild = false;

                    else if (
                        rayPosition.x < -1 * gridOffset || 
                        rayPosition.y > _gridController.gridSize.x * gridOffset - _draggingBuilding.GetComponent<GridBuilding>().buildingSize.x
                        )
                        _isAvailableToBuild = false;

                    else if (
                        rayPosition.y < -1 * gridOffset || 
                        rayPosition.y > _gridController.gridSize.y * gridOffset - _draggingBuilding.GetComponent<GridBuilding>().buildingSize.y
                        )
                        _isAvailableToBuild = false;

                    else if (
                        _gridController.IsPlaceTaken(
                        (int)_draggingBuilding.transform.position.x / gridOffset,
                        (int)_draggingBuilding.transform.position.z / gridOffset)
                        )
                        _isAvailableToBuild = false;
                        
                    else
                        _isAvailableToBuild = true;

                    _draggingBuilding.transform.position = new Vector3(_terrain.gameObject.transform.position.x, y, _terrain.gameObject.transform.position.z);
                    _draggingBuilding.GetComponent<GridBuilding>().SetColor(_isAvailableToBuild);
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isAvailableToBuild)
        {
            Vector2Int pos = new Vector2Int((int)_draggingBuilding.transform.position.x / gridOffset, (int)_draggingBuilding.transform.position.z / gridOffset);
            _draggingBuilding.GetComponent<GridBuilding>().ResetColor();
            _gridController.SetGridSlot(pos, _terrain);
            _gridController.TryPlace(pos, _draggingBuilding.GetComponent<Building>());

        }
        Destroy(_draggingBuilding);
    }
}