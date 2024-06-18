using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDetailHandler : MonoBehaviour
{
    private Camera _camera;

    [SerializeField, ReadOnly]
    private bool _showingBuildingDetail;

    private Building _currentBuilding;

    LayerMask buildingMask;

    [Header("UI Assets")]
    [SerializeField] private GameObject _buildingDetailRoot;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private CardStatDisplay _cardStatDisplay;
    [SerializeField]
    private Button _sellButton;
    [SerializeField]
    private Button _closeButton;


    void Awake()
    {
        //Maska nie dziala idk czemu w tym Raycastcie
        buildingMask = LayerMask.NameToLayer("Building") | LayerMask.NameToLayer("MainBuilding");
        _camera = Camera.main;
        _buildingDetailRoot.SetActive(false);

        _closeButton.onClick.AddListener(CloseDetailPanel);
        _sellButton.onClick.AddListener(SellBuilding);
    }

    public void CloseDetailPanel()
    {
        _showingBuildingDetail = false;
        _buildingDetailRoot.SetActive(false);
        _currentBuilding = null;
    }


    void Update()
    {
        if (DayNightCycleManager.Instance.IsDay && !UIFlow.Instance.IsPlacingCard())
        {
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f))
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        ShowBuildingDetail(hit.collider.gameObject.GetComponentInParent<Building>());
                        return;
                    }
                }
                //if(_showingBuildingDetail == true)
                //{
                //    CloseDetailPanel();
                //}
            }
        }
    }

    public void ShowBuildingDetail(Building b)
    {
        _currentBuilding = b;
 
        _showingBuildingDetail = true;

        _buildingDetailRoot.SetActive(true);

        _title.text = b.GetBaseStatistics().Name;
        _cardStatDisplay.Display(b.GetBaseStatistics().GetStatistics());
    }

    public void SellBuilding()
    {
        if (_currentBuilding == null)
            return;

        
        _currentBuilding.OnSell();
        GridController.Instance.RemoveBuilding(_currentBuilding.GetComponent<GridBuilding>().gridPos);
        _currentBuilding = null;
        CloseDetailPanel();
    }
}
