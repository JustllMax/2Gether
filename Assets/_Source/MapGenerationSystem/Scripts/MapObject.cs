using UnityEngine;

[CreateAssetMenu(menuName = "Map/new map", fileName = "New map", order = 52)]
public class MapObject : ScriptableObject
{
    [SerializeField] public Vector2Int mapSize = new Vector2Int(13, 13);
    [SerializeField] public Vector2Int startPos = new Vector2Int(6, 6);
    [SerializeField] public int[,] map;

    void OnEnable()
    {
        if (map == null || map.GetLength(0) != mapSize.x || map.GetLength(1) != mapSize.y)
        {
            map = new int[mapSize.x, mapSize.y];
        }
    }
}
