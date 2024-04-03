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
    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }

    

    public Way(Way newWay)
    {
        this.id = newWay.id;
        this.pos = newWay.pos;
        this.wayAnchors = newWay.wayAnchors;
    }
}
