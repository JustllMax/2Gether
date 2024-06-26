using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BuildingDetailHandler : MonoBehaviour
{
    private Camera _camera;

    [SerializeField, ReadOnly]
    private bool _showingBuildingDetail;

    private Building _currentBuilding;

    [Header("UI Assets")]
    [SerializeField] private GameObject _buildingDetailRoot;
    //ui stuff
    [SerializeField]
    private TMP_Text _title;
    [SerializeField]
    private TMP_Text _description;
    [SerializeField]
    List<Image> levelMarks;
    [SerializeField]
    private Image _thumbnail;
    [SerializeField]
    private Image _backgroundRaycastTarget;

    [SerializeField]
    Color whiteColor;
    [SerializeField]
    Color blueColor;
    [SerializeField]
    Color redColor;

    [SerializeField]
    private CardStatDisplay _cardStatDisplay;

    [SerializeField]
    private Image _activeBorder;

    [SerializeField]
    private Image _typeIcon;
    [SerializeField]
    List<Sprite> symbolTypeIcons;
    [SerializeField]
    private Button _sellButton;
    [SerializeField]
    private Image _sellButtonBackground;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Image _closeButtonBackground;

    private GraphicRaycaster uiRaycaster;
    void Awake()
    {
        
        _camera = Camera.main;
        _buildingDetailRoot.SetActive(false);

        _closeButton.onClick.AddListener(CloseDetailPanel);
        _sellButton.onClick.AddListener(SellBuilding);
        uiRaycaster = GetComponent<GraphicRaycaster>();
    }

    public void CloseDetailPanel()
    {
        BuildingPlacer.Instance.DisperseRangeCircle();
        _showingBuildingDetail = false;
        _buildingDetailRoot.SetActive(false);
        _cardStatDisplay.Clean();
        _currentBuilding = null;
    }


    void Update()
    {
        if (DayNightCycleManager.Instance.IsDay && !UIFlow.Instance.IsPlacingCard())
        {

            if (Input.GetMouseButtonDown(0))
            {


                List<RaycastResult> results = new List<RaycastResult>();

                RaycastUI(results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("DetailsWindow"))
                    {
                        return;
                    }
                }

                RaycastHit hit;

                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 9999.0f))
                {
                    if (hit.collider.gameObject.CompareTag("Building"))
                    {
                        ShowBuildingDetail(hit.collider.gameObject.GetComponentInParent<Building>());
                        return;
                    }
                }
                CloseDetailPanel();
            }
        }

        if (_showingBuildingDetail)
        {
            BuildingPlacer.Instance.SummonRangeCircle(_currentBuilding.transform.position, ((BuildingOffensiveStatistics)_currentBuilding.GetBaseStatistics()).AttackRange);
        }
    }

    public void ShowBuildingDetail(Building b)
    {
        _currentBuilding = b;
        var stats = b.GetBaseStatistics();
        _showingBuildingDetail = true;

        _buildingDetailRoot.SetActive(true);

        _title.text = b.GetBaseStatistics().Name;
        _activeBorder.color = GetColorForRarity(stats.Rarity);
        _title.text = stats.Name;
        _description.text = stats.Description;
        _thumbnail.sprite = stats.Thumbnail;
        _backgroundRaycastTarget.color = _activeBorder.color;
        EnableLevelMarks(stats.Rarity);
        SetTypeIcon(stats.BuildingType);
        _cardStatDisplay.SetUpDisplay(stats.GetStatistics());
        _closeButtonBackground.color = _activeBorder.color;
        _sellButtonBackground.color = _activeBorder.color;
    }

    public void SellBuilding()
    {
        if (_currentBuilding == null)
            return;


        AudioManager.Instance.PlaySFX("A_DayUI_Sell_Cards");
        
        _currentBuilding.OnSell();
        GridController.Instance.RemoveBuilding(_currentBuilding.GetComponent<GridBuilding>().gridPos);
        _currentBuilding = null;
        CloseDetailPanel();
    }

    void SetTypeIcon(BuildingType type)
    {
        _typeIcon.sprite = symbolTypeIcons[(int)type];
    }

    void EnableLevelMarks(Rarity r)
    {

        for (int i = 0; i < levelMarks.Count; i++)
        {
            levelMarks[i].gameObject.SetActive(false);
        }
        int level = (int)r;
        //base level is 0
        for (int i = 0; i <= level; i++)
        {
            levelMarks[i].gameObject.SetActive(true);
            levelMarks[i].color = _activeBorder.color;
        }
    }

    private Color GetColorForRarity(Rarity r)
    {
        switch (r)
        {
            case Rarity.Generic:
                return whiteColor;
                break;
            case Rarity.Enhanced:
                return blueColor;
                break;
            case Rarity.Prototype:
                return redColor;
                break;
            default: return whiteColor;
        }

    }

    private void RaycastUI(List<RaycastResult> results)
    {

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        uiRaycaster.Raycast(pointerEventData, results);
    }
}
