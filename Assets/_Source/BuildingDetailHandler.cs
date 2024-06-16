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

    // Start is called before the first frame update
    void Awake()
    {
        _camera = Camera.main;
        _buildingDetailRoot.SetActive(false);

        _closeButton.onClick.AddListener(CloseDetailPanel);
    }

    public void CloseDetailPanel()
    {
        _showingBuildingDetail = false;
        _buildingDetailRoot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DayNightCycleManager.Instance.IsDay && !_showingBuildingDetail && !UIFlow.Instance.IsPlacingCard())
        {
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f))
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        ShowBuildingDetail(hit.collider.gameObject.GetComponentInParent<Building>());
                    }
                }
            }
        }
    }

    public void ShowBuildingDetail(Building b)
    {
        if (_showingBuildingDetail)
            return;

        _showingBuildingDetail = true;

        _buildingDetailRoot.SetActive(true);

        _title.text = b.GetBaseStatistics().Name;
        _cardStatDisplay.Display(b.GetBaseStatistics().GetStatistics());
    }
}
