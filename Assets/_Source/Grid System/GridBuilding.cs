using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : Building
{
    [SerializeField] private Vector3 _buildingSize;
    [SerializeField] private Renderer _renderer;

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

    public override void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
