using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Slot
{
    [SerializeField] public int id;
    [SerializeField] public SlotAnchor[] slotAnchors;
    private Vector2Int _pos;
    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }
    public Slot(Slot slot)
    {
        id = slot.id;
        slotAnchors = slot.slotAnchors;
        _pos = slot.pos;
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
}
