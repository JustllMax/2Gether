using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private  GameObject _terrain;
    #endregion

    #region Grid
    private GridController _gridController;
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(12, 12);
    private bool _isAvailableToBuild;
    #endregion

    private Vector2Int rayPosition;
    private void Awake()
    {
        _gridController = GridController.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggingBuilding = Instantiate(_cardSO.prefab, Vector3.zero, Quaternion.identity);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Terrain"))
            {
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
                    _terrain = hit.collider.gameObject;
                    rayPosition.x = Mathf.RoundToInt(hit.point.x);
                    rayPosition.y = Mathf.RoundToInt(hit.point.z);

                    if (rayPosition.x < -5 || rayPosition.y > _gridController.gridSize.x * 10 -  _draggingBuilding.GetComponent<GridBuilding>().buildingSize.x)
                        _isAvailableToBuild = false;
                    else if (rayPosition.y < -5 || rayPosition.y > _gridController.gridSize.y * 10 -  _draggingBuilding.GetComponent<GridBuilding>().buildingSize.y)
                        _isAvailableToBuild = false;
                    else if (_gridController.IsPlaceTaken((int)_draggingBuilding.transform.position.x / 10, (int)_draggingBuilding.transform.position.z / 10))
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
        if(_isAvailableToBuild)
        {
            Vector2Int pos = new Vector2Int((int)_draggingBuilding.transform.position.x / 10, (int)_draggingBuilding.transform.position.z / 10);
             _draggingBuilding.GetComponent<GridBuilding>().ResetColor();
            _gridController.SetGridSlot(pos, _terrain);
            _gridController.TryPlace(pos, _draggingBuilding.GetComponent<Building>());
        }
        Destroy(_draggingBuilding);
    }
}