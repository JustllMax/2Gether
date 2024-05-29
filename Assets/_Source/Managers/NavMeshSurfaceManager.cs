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

    void Awake()
    {
        SlotPlacer.OnMapGenerated += OnMapGenerated;
        
    }
    void OnDestroy()
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

        OnNavMeshGenerated?.Invoke();
    }
}
