using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour
{
    [SerializeField] private Vector3 _buildingSize;
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private bool _isCanBePlacedOnRoad = false;
    public bool isCanBePlacedOnRoad {get{ return _isCanBePlacedOnRoad; } set { _isCanBePlacedOnRoad = value; } }
    private Vector2Int _gridPos;
    public Vector2Int gridPos{get{return _gridPos;} set{_gridPos = value;} }
    private List<Color> _rootColor; 
    private bool _isDecorationCollision;
    public bool IsDecorationCollision
    {
        get { return _isDecorationCollision; }
    }
    public Vector3 buildingSize { get => _buildingSize; set {; } }

    void Awake()
    {
        _rootColor = new List<Color>();
        foreach(var renderer in _renderers)
        {
            _rootColor.Add(renderer.material.color);
        }
    }
    public void SetColor(bool isAvailableToBuild)
    {
        
        if (isAvailableToBuild)
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = Color.green;
            }
        }
        else
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = Color.red;
            }
        }
    }

    public void ResetColor()
    {
        for(int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.color = _rootColor[i];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter " + other.gameObject.name);
        if (other.gameObject.tag == "Decoration")
        {
            _isDecorationCollision = true;
            Debug.Log("Decoration " + _isDecorationCollision);
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit" + other.gameObject.name);
        if (other.gameObject.tag == "Decoration")
        {
            _isDecorationCollision = false;
            Debug.Log("Decoration " + _isDecorationCollision);
        }
    }
}
