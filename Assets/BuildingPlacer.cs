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


    private BuildingCard _selectedBuildingCard;
    private GameContext _gameContext;

    [SerializeField]
    private GridController _gridController;

    [SerializeField]
    private GameObject _rangeIndicatorPrefab;

    private GameObject _rangeIndicatorInstance;

    private void Start()
    {
        _rangeIndicatorInstance = Instantiate(_rangeIndicatorPrefab);
        _rangeIndicatorInstance.transform.position = Vector3.up * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlacing)
        {
            _rangeIndicatorInstance.transform.position = _draggingBuilding.transform.position;
            _rangeIndicatorInstance.transform.localScale = Vector3.one * ((BuildingOffensiveStatistics)_selectedBuildingCard.CardStatisticsData).AttackRange;

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
                        _selectedBuildingCard.OnCardSubmitted(_gameContext);
                        EndPlaceMode();
                    }
                }
            }
        }
    }

    public void StartPlaceMode(BuildingCard bc, GameContext ctx)
    {
        _selectedBuildingCard = bc;
        _gameContext = ctx;
        _isPlacing = true;
        _draggingBuilding = Instantiate(bc.BuildingPrefab);
    }

    public void EndPlaceMode()
    {
        _isPlacing = false;
        _rangeIndicatorInstance.transform.position = Vector3.up * 1000;
        Destroy( _draggingBuilding );
    }
}
