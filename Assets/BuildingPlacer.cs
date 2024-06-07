using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using TMPro;



public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance;

    public void Awake()
    {
        Instance = this;
    }

    private bool _isPlacing;

   

    private float y;
    GameObject _terrain;
    private Vector2Int rayPosition;
    public LayerMask layerMask;
    private bool _isAvailableToBuild;
    private GameObject _draggingBuilding;
    [SerializeField] private const int gridOffset = 10;


    [SerializeField]
    private GridController _gridController;

    // Update is called once per frame
    void Update()
    {
        if (_isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Terrain") || hit.collider.gameObject.CompareTag("Way"))
                {
                    _terrain = hit.collider.gameObject;
                    rayPosition.x = Mathf.RoundToInt(math.abs(hit.point.x));
                    rayPosition.y = Mathf.RoundToInt(math.abs(hit.point.z));



                    if (_draggingBuilding.GetComponent<GridBuilding>().IsDecorationCollision)
                        _isAvailableToBuild = false;
                    else if (_terrain.CompareTag("Way") && !_draggingBuilding.GetComponent<GridBuilding>().isCanBePlacedOnRoad)
                    {

                        _isAvailableToBuild = false;
                    }
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

            if (Input.GetMouseButtonDown(0))
            {
                if (_isAvailableToBuild)
                {
                    Vector2Int pos = new Vector2Int((int)_draggingBuilding.transform.position.x / gridOffset, (int)_draggingBuilding.transform.position.z / gridOffset);
                    _draggingBuilding.GetComponent<GridBuilding>().ResetColor();
                    _gridController.SetGridSlot(pos, _terrain);
                    if (_gridController.TryPlace(pos, _draggingBuilding.GetComponent<Building>()))
                    {
                        Debug.Log("Place ok");
                        Destroy(_draggingBuilding);
                    }
                }
            }
        }
    }

    public void StartPlaceMode(BuildingCard bc)
    {
        _isPlacing = true;
        _draggingBuilding = Instantiate(bc.BuildingPrefab);
    }

    public void EndPlaceMode()
    {
        _isPlacing = false;
        Destroy( _draggingBuilding );
    }
}
