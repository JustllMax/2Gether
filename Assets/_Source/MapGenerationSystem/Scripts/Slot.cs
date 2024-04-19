using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Slot/new slot", fileName = "New slot", order = 52)]
public class Slot
{
    #region Variables
    [SerializeField] public int id;
    private List<SlotAnchor> _slotAnchors = new List<SlotAnchor>();
    private Vector2Int _pos;

    #region Getters and setters
    public List<SlotAnchor> slotAnchors
    {
        get => _slotAnchors;
        set { _slotAnchors = value; }
    }
    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }
    #endregion
    
    #endregion

    #region Constructors
    public Slot()
    {
        id = -1;
        _slotAnchors = new List<SlotAnchor>();
        _pos = new Vector2Int(0,0);
    }
    public Slot(Slot slot)
    {
        id = slot.id;
        slotAnchors = slot.slotAnchors;
        _pos = slot.pos;
    }
    public Slot(int id, Vector2Int pos, GameObject slots)
    {
        this.id = id;
        this._pos = pos;
        
        _slotAnchors.Clear();
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            _slotAnchors.Add(slots.transform.GetChild(i).GetComponent<SlotAnchor>());
        }
    }
    #endregion

    #region Public Methods
    public void SetSlotAnchors(GameObject slots)
    {
        _slotAnchors.Clear();
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            _slotAnchors.Add(slots.transform.GetChild(i).GetComponent<SlotAnchor>());
        }
    }
    public void RotateAnchors(float angle)
    {
        if (id == 1)
        {
            if (angle == 0f || angle == 180f)
            {

                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = false;
                slotAnchors[2].IsWay = true;
                slotAnchors[3].IsWay = false;
            }
            if (angle == 90f || angle == 270f)
            {
                slotAnchors[0].IsWay = false;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = false;
                slotAnchors[3].IsWay = true;
            }
        }
        if (id == 2)
        {
            if (angle == 0)
            {
                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = false;
                slotAnchors[3].IsWay = true;
            }
            if (angle == 90)
            {
                slotAnchors[0].IsWay = false;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = true;
                slotAnchors[3].IsWay = false;
            }
            if (angle == 180)
            {
                slotAnchors[0].IsWay = false;
                slotAnchors[1].IsWay = false;
                slotAnchors[2].IsWay = true;
                slotAnchors[3].IsWay = true;
            }
            if (angle == 270)
            {
                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = false;
                slotAnchors[2].IsWay = false;
                slotAnchors[3].IsWay = true;
            }
        }
        if (id == 3)
        {
            if (angle == 0)
            {
                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = false;
                slotAnchors[3].IsWay = true;
            }
            if (angle == 90)
            {
                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = false;
                slotAnchors[3].IsWay = true;
            }
            if (angle == 180)
            {
                slotAnchors[0].IsWay = true;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = true;
                slotAnchors[3].IsWay = false;
            }
            if (angle == 270)
            {
                slotAnchors[0].IsWay = false;
                slotAnchors[1].IsWay = true;
                slotAnchors[2].IsWay = true;
                slotAnchors[3].IsWay = true;
            }
        }
    }
    #endregion
}
