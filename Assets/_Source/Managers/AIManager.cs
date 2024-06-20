using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class AIManager : MonoBehaviour
{
    private static AIManager _instance;
    public static AIManager Instance { get { return _instance; } }

    private LayerMask[] targetMasks;
    private int[] agentTypeIDs;

    [SerializeField] private int ticksPerSecond = 5;

    public LinkedList<AIController> enemies;
    private LinkedListNode<AIController> currentNode;
    private float tickTime = 0f;
    private float enemiesPerFrame;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    
    private void Start()
    {
        enemies = new LinkedList<AIController>();

        targetMasks = new LayerMask[4];
        targetMasks[0] = LayerMask.GetMask("Building");
        targetMasks[1] = LayerMask.GetMask("Player");
        targetMasks[2] = LayerMask.GetMask("MainBuilding");
        targetMasks[3] = LayerMask.GetMask("Building", "MainBuilding");

        agentTypeIDs = new int[6];
        agentTypeIDs[0] = GetNavMeshAgentID(NavAgentType.Small.ToString() + "Full") ?? -1;
        agentTypeIDs[1] = GetNavMeshAgentID(NavAgentType.Small.ToString() + "Path") ?? -1;

        agentTypeIDs[2] = GetNavMeshAgentID(NavAgentType.Medium.ToString() + "Full") ?? -1;
        agentTypeIDs[3] = GetNavMeshAgentID(NavAgentType.Medium.ToString() + "Path") ?? -1;

        agentTypeIDs[4] = GetNavMeshAgentID(NavAgentType.Large.ToString() + "Full") ?? -1;
        agentTypeIDs[5] = GetNavMeshAgentID(NavAgentType.Large.ToString() + "Path") ?? -1;
    }
    
    private void Update()
    {

    }

    public LinkedListNode<AIController> RegisterEnemy(AIController enemy)
    {
        var node = enemies.AddFirst(enemy);

        if (enemies.Count == 1)
            currentNode = node;

        return node;
    }
    public void UnregisterEnemy(LinkedListNode<AIController> enemyNode)
    {
        if (currentNode == enemyNode)
            currentNode = currentNode.Next ?? enemies.First;

        enemies.Remove(enemyNode);
    }

    public LayerMask GetLayerFromType(TargetType type)
    {
        return targetMasks[(int)type];
    }

    public int GetAgentIDFromType(NavAgentType type, bool walkOnPath)
    {
        return agentTypeIDs[(int)type * 2 + (walkOnPath ? 1 : 0)];
    }

    private int? GetNavMeshAgentID(string name)
    {
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
            if (NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID) == name)
            {
                return settings.agentTypeID;
            }
        }
        return null;
    }

    void FixedUpdate()
    {
        if (enemies == null || enemies.Count == 0)
            return;

        float enemiesPerUpdate = enemies.Count * ticksPerSecond * Time.fixedDeltaTime;
        tickTime += enemiesPerUpdate;
        if (tickTime >= 1f) 
        {
            while (tickTime >= 1f)
            {
                tickTime -= 1f;
                currentNode.Value.OnTick(Time.fixedTimeAsDouble);
                currentNode = currentNode.Next ?? enemies.First;
            }
        }
    }

    public bool SampleNavSurface(in Vector3 point, float maxDistance, NavAgentType type, bool samplePath, out Vector3 pointOnSurface)
    {
        int pathAgentType = GetAgentIDFromType(type, samplePath);
        if (NavMesh.SamplePosition(point, out NavMeshHit navhit, maxDistance, new NavMeshQueryFilter { agentTypeID = pathAgentType, areaMask = NavMesh.AllAreas }))
        {
            pointOnSurface = navhit.position;
            return true;
        }

        pointOnSurface = Vector3.zero;
        return false;
    }

}
