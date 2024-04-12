using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Way : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] public WayAnchor[] wayAnchors;
    private Vector2Int _pos;
    private GameObject[] WayAnchorsObjects;
    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }
    public void RotateWay(Vector3 axis, float rotationAngle)
    {
        gameObject.transform.GetChild(0).Rotate(axis, rotationAngle);
        float angle = gameObject.transform.GetChild(0).eulerAngles.y;
        GameObject anchors = gameObject.transform.GetChild(1).gameObject;
        if (id == 1)
        {
            if (angle == 0f || angle == 180f)
            {
                Debug.Log("ANGLE!");
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = false;
            }
            if (angle == 90f || angle == 270f)
            {
                Debug.Log("ANGLE!");
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
        }
        if (id == 2)
        {
            if (angle == 0)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
            if (angle == 90)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = false;
            }
            if (angle == 180)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
            if (angle == 270)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
        }
        if (id == 3)
        {
            if (angle == 0)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
            if (angle == 90)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
            if (angle == 180)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = false;
            }
            if (angle == 270)
            {
                anchors.transform.GetChild(0).GetComponent<WayAnchor>().IsWay = false;
                anchors.transform.GetChild(1).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(2).GetComponent<WayAnchor>().IsWay = true;
                anchors.transform.GetChild(3).GetComponent<WayAnchor>().IsWay = true;
            }
        }
    }
}
