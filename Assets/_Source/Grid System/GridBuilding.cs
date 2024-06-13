using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour
{
    [SerializeField] private Vector3 _buildingSize;
    [SerializeField] private Renderer _renderer;
    [SerializeField] public bool isCanBePlacedOnRoad = false;
    private bool _isDecorationCollision;
    public bool IsDecorationCollision
    {
        get { return _isDecorationCollision; }
    }
    public Vector3 buildingSize {get => _buildingSize; set{;}}

    public void SetColor(bool isAvailableToBuild)
    {
        if(isAvailableToBuild)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }

    public void ResetColor()
    {
        _renderer.material.color = Color.white;
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
