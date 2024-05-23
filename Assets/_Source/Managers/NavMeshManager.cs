using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface[] navMeshSurfaces;

    void Start()
    {
        BakeAllNavMeshes();
    }

    public void BakeAllNavMeshes()
    {
        foreach (var surface in navMeshSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
}
