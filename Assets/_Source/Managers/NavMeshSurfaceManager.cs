using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshSurfaceManager : MonoBehaviour
{
    [SerializeField] public List<NavMeshSurface> surfaces;

    public delegate void NavMeshGeneratedHandler();
    public static event Action OnNavMeshGenerated;

    void OnEnable()
    {
        SlotPlacer.OnMapGenerated += OnMapGenerated;
    }
    void OnDisable()
    {
        SlotPlacer.OnMapGenerated -= OnMapGenerated;
    }
    public void BakeAllNavMeshes()
    {
        foreach (var surface in surfaces)
        {
            surface.BuildNavMesh();
        }
       
    }

    void OnMapGenerated()
    {
        BakeAllNavMeshes();

        Debug.Log("NavMesh Invoke");
        OnNavMeshGenerated?.Invoke();

    }
}
