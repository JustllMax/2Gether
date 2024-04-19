using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class WaySlot : MonoBehaviour
{
    public Slot slot;
    public void RotateWays(Vector3 axis, float rotationAngle)
    {
        gameObject.transform.GetChild(0).Rotate(axis, rotationAngle);;
    }
    public void RotateSlot(Vector3 axis, float rotationAngle)
    {
        gameObject.transform.GetChild(1).Rotate(axis, rotationAngle);
        float angle = gameObject.transform.GetChild(0).eulerAngles.y;
        slot.RotateAnchors(angle);
    }
}
