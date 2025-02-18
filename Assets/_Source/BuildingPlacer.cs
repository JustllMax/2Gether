using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;



public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance;

    public void Awake()
    {
        Instance = this;
    }

    private bool _isPlacing;

   

    private float y = 0f;
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

    public void SummonRangeCircle(Vector3 position, float range)
    {
        _rangeIndicatorInstance.transform.position = position;
        _rangeIndicatorInstance.transform.localScale = Vector3.one * range;
    }

    public void DisperseRangeCircle()
    {
        _rangeIndicatorInstance.transform.position = Vector3.up * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlacing)
        {

            SummonRangeCircle(_draggingBuilding.transform.position, ((BuildingOffensiveStatistics)_selectedBuildingCard.CardStatisticsData).AttackRange);

            if(Input.GetKeyUp(KeyCode.R))
            {
                _draggingBuilding.transform.Rotate(Vector3.up, 90f);
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask) && !EventSystem.current.IsPointerOverGameObject())
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

                    //else if (
                    //    _gridController.IsPlaceTaken(
                    //    (int)_draggingBuilding.transform.position.x / gridOffset,
                    //    (int)_draggingBuilding.transform.position.z / gridOffset)
                    //    )
                    //    _isAvailableToBuild = false;

                    else
                        _isAvailableToBuild = true;

                    _draggingBuilding.transform.position = new Vector3(_terrain.gameObject.transform.position.x, y, _terrain.gameObject.transform.position.z);
                    _draggingBuilding.GetComponent<GridBuilding>().SetColor(_isAvailableToBuild);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (_isAvailableToBuild)
                    {
                        Vector2Int pos = new Vector2Int((int)_draggingBuilding.transform.position.x / gridOffset, (int)_draggingBuilding.transform.position.z / gridOffset);
                        _draggingBuilding.GetComponent<GridBuilding>().ResetColor();
                        _draggingBuilding.GetComponent<GridBuilding>().gridPos = pos;
                        _gridController.SetGridSlot(pos, _terrain);


                        if (_gridController.TryPlace(pos, _draggingBuilding.GetComponent<Building>(), out Building alreadyPlacedBuilding))
                        {
                            Debug.Log("Place ok");

                            AudioManager.Instance.PlaySFX("A_DayUI_Set_Building");

                            _selectedBuildingCard.OnCardSubmitted(_gameContext);
                            EndPlaceMode();
                        }
                        else
                        {
                            var newBuilding = _draggingBuilding.GetComponent<Building>();

                            Debug.Log("Place taken, checking upgrade conditions");

                            // check if we try to place the same building type
                            if (newBuilding.GetBaseStatistics().GetType() == alreadyPlacedBuilding.GetBaseStatistics().GetType())
                            {
                                if (newBuilding.GetBaseStatistics().Rarity > alreadyPlacedBuilding.GetBaseStatistics().Rarity)
                                {
                                    // better rarity, replace
                                    _gridController.RemoveBuilding(pos);
                                    _gridController.TryPlace(pos, newBuilding, out _);

                                    AudioManager.Instance.PlaySFX("A_DayUI_Upgrade_Building");

                                    _selectedBuildingCard.OnCardSubmitted(_gameContext);
                                    EndPlaceMode();
                                }
                                else if (newBuilding.GetBaseStatistics().Rarity == alreadyPlacedBuilding.GetBaseStatistics().Rarity)
                                {
                                    // upgrade
                                    alreadyPlacedBuilding.TryUpgrading(alreadyPlacedBuilding.GetBaseStatistics());

                                    AudioManager.Instance.PlaySFX("A_DayUI_Upgrade_Building");

                                    _selectedBuildingCard.OnCardSubmitted(_gameContext);
                                    EndPlaceMode();
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    GameObject SpawnBuildingWithLevel(GameObject prefab, Rarity r)
    {
        var go = Instantiate(prefab);
        go.GetComponent<Building>().Init((int)r);
        return go;
    }

    public void StartPlaceMode(BuildingCard bc, GameContext ctx)
    {
        _selectedBuildingCard = bc;
        _gameContext = ctx;
        _isPlacing = true;
        _draggingBuilding = SpawnBuildingWithLevel(bc.BuildingPrefab, bc.CardStatisticsData.Rarity);
    }

    public void EndPlaceMode()
    {
        _isPlacing = false;
        DisperseRangeCircle();
        Destroy( _draggingBuilding );
    }
}
