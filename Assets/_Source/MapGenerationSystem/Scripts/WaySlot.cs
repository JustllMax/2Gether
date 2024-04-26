using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Android;

public class WaySlot : MonoBehaviour
{
    [SerializeField] public Slot slot;
    [SerializeField] public bool[] slotAnchors = new bool[4];

    public void RotateWays(Vector3 axis, float rotationAngle)
    {
        gameObject.transform.GetChild(0).Rotate(axis, rotationAngle); ;
    }

    public void RotateAnchors(float angle)
    {
        if (slot.id == 1)
        {
            if (angle == 0f || angle == 180f)
            {

                slotAnchors[0] = true;
                slotAnchors[1] = false;
                slotAnchors[2] = true;
                slotAnchors[3] = false;
            }
            if (angle == 90f || angle == 270f)
            {
                slotAnchors[0] = false;
                slotAnchors[1] = true;
                slotAnchors[2] = false;
                slotAnchors[3] = true;
            }
        }
        if (slot.id == 2)
        {
            if (angle == 0)
            {
                slotAnchors[0] = true;
                slotAnchors[1] = true;
                slotAnchors[2] = false;
                slotAnchors[3] = false;
            }
            if (angle == 90)
            {
                slotAnchors[0] = false;
                slotAnchors[1] = true;
                slotAnchors[2] = true;
                slotAnchors[3] = false;
            }
            if (angle == 180)
            {
                slotAnchors[0] = false;
                slotAnchors[1] = false;
                slotAnchors[2] = true;
                slotAnchors[3] = true;
            }
            if (angle == 270)
            {
                slotAnchors[0] = true;
                slotAnchors[1] = false;
                slotAnchors[2] = false;
                slotAnchors[3] = true;
            }
        }
        if (slot.id == 3)
        {
            if (angle == 0)
            {
                slotAnchors[0] = true;
                slotAnchors[1] = true;
                slotAnchors[2] = false;
                slotAnchors[3] = true;
            }
            if (angle == 90)
            {
                slotAnchors[0] = true;
                slotAnchors[1] = true;
                slotAnchors[2] = false;
                slotAnchors[3] = true;
            }
            if (angle == 180)
            {
                slotAnchors[0] = true;
                slotAnchors[1] = true;
                slotAnchors[2] = true;
                slotAnchors[3] = false;
            }
            if (angle == 270)
            {
                slotAnchors[0] = false;
                slotAnchors[1] = true;
                slotAnchors[2] = true;
                slotAnchors[3] = true;
            }
        }
    }
    public void RotateSlot(Vector3 axis, float rotationAngle)
    {
        this.RotateWays(axis, rotationAngle);
        this.RotateAnchors(rotationAngle);
    }
}
