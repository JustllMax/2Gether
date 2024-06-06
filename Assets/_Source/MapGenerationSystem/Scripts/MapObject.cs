using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Map/new map", fileName = "New map", order = 52)]
public class MapObject : ScriptableObject
{
    [SerializeField] public Vector2Int mapSize = new Vector2Int(13, 13);
    [SerializeField] public Vector2Int startPos = new Vector2Int(6, 6);
    [SerializeField, HideInInspector] private int[] serializedMap;

    [NonSerialized] public int[,] map;

    void OnEnable()
    {
        if (map == null || map.Length != mapSize.x * mapSize.y)
        {
            map = new int[mapSize.x, mapSize.y];
            if (serializedMap != null && serializedMap.Length == mapSize.x * mapSize.y)
            {
                for (int x = 0; x < mapSize.x; x++)
                {
                    for (int y = 0; y < mapSize.y; y++)
                    {
                        map[x, y] = serializedMap[y * mapSize.x + x];
                    }
                }
            }
        }
    }

    public void OnBeforeSerialize()
    {
        serializedMap = new int[mapSize.x * mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                serializedMap[y * mapSize.x + x] = map[x, y];
            }
        }
    }

    public void OnAfterDeserialize()
    {
        map = new int[mapSize.x, mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                map[x, y] = serializedMap[y * mapSize.x + x];
            }
        }
    }
}
