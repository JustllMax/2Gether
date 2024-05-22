using System.Collections.Generic;
using UnityEngine;


public class SlotPlacer : MonoBehaviour
{

    #region slots
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private PathSlot startingSlot;

    #endregion

    #region map

    [SerializeField] private List<MapObject> maps;
    [SerializeField] private int mapId;
    [SerializeField] private bool isRandomGeneration = false;

    #endregion

    #region private variables

    private float[] _angles = { 0f, 90f, 180f, 270f };

    private Vector2Int mapSize;
    private Vector2Int startPos;

    public PathSlot[,] spawnedSlots;
    private int[,] _map;
    #endregion


    void Start()
    {
        mapSize = maps[mapId].mapSize;
        startPos = maps[mapId].startPos;
        Debug.Log("" + mapSize.x);
        Generate();
    }

    void Generate()
    {
        spawnedSlots = new PathSlot[mapSize.x, mapSize.y];
        if (isRandomGeneration)
        {
            PathGenerator pathGenerator = new PathGenerator(mapSize, startPos);
            pathGenerator.GenerateMap();
            _map = (int[,])pathGenerator.map.Clone();
        }
        else
            _map = (int[,])maps[mapId].map.Clone();

            string line = "";
            for(int i = 0; i < _map.GetLength(0); i++)
            {
                for(int j = 0; j < _map.GetLength(1); j++)
                {
                    line += _map[i,j] + "\t";
                }
                line += "\n\n";
            }
            Debug.Log(line);
        GenerateMap();
    }

    void GenerateMap()
    {
        PathSlot path;
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                if (y == startPos.x && x == startPos.y)
                {
                    path = Instantiate(startingSlot).GetComponent<PathSlot>();
                    path.gameObject.transform.position = new Vector3(x, 0, y) * 30;
                    path.slot.pos = new Vector2Int(x, y);
                    spawnedSlots[x, y] = path;
                    path.gameObject.transform.SetParent(this.transform);
                }
                else
                {
                    if (_map[x, y] == 2)
                    {
                        if (isCurved(x, y))
                            path = Instantiate(slotPrefabs[5]).GetComponent<PathSlot>();
                        else
                            path = Instantiate(slotPrefabs[2]).GetComponent<PathSlot>();
                    }
                    else
                        path = Instantiate(slotPrefabs[_map[x, y]]).GetComponent<PathSlot>();

                    path.gameObject.transform.position = new Vector3(x, 0, y) * 30;
                    path.slot.pos = new Vector2Int(x, y);

                    #region parent   

                    Debug.Log($"x:{path.slot.pos.x} y: {path.slot.pos.y}");

                    if (
                        path.slot.pos.x >= 0 && path.slot.pos.x < startPos.x &&
                        path.slot.pos.y >= startPos.y && path.slot.pos.y <= mapSize.y
                        )
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(0));
                    }
                    else if (
                        path.slot.pos.x >= startPos.x && path.slot.pos.x < mapSize.x &&
                        path.slot.pos.y > startPos.y && path.slot.pos.y < mapSize.y
                        )
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(1));
                    }
                    else if (
                        path.slot.pos.x > startPos.x && path.slot.pos.x < mapSize.x &&
                        path.slot.pos.y >= 0 && path.slot.pos.y <= startPos.y
                        )
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(2));
                    }
                    else if (
                        path.slot.pos.x >= 0 && path.slot.pos.x <= startPos.x &&
                        path.slot.pos.y >= 0 && path.slot.pos.y < startPos.y
                        )
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(3));
                    }
                    else
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(4));
                    }
                    #endregion
                }



                RotatePath(x, y, path);
                spawnedSlots[x, y] = path;

            }
        }
    }

    void RotatePath(int x, int y, PathSlot path)
    {
        int neighborDir = 0;
        if (x >= 0 && x < mapSize.x && y >= 0 && y < mapSize.y)
        {
            if (_map[x, y] == 1)
            {
                if (y == 0 && _map[x, y + 1] != 0)
                {
                    neighborDir = 1;
                }
                if (y == mapSize.y - 1 && _map[x, y - 1] != 0)
                {
                    neighborDir = 3;
                }

                if (x == mapSize.x - 1 && _map[x - 1, y] != 0)
                {
                    neighborDir = 0;
                }
                if (x == 0 && _map[x + 1, y] != 0)
                {
                    neighborDir = 2;
                }
            }
            if (_map[x, y] == 2 && isCurved(x, y))
            {
                if (_map[x - 1, y] != 0 && _map[x, y - 1] != 0)
                {
                    neighborDir = 3;
                }
                if (_map[x - 1, y] != 0 && _map[x, y + 1] != 0)
                {
                    neighborDir = 0;
                }
                if (_map[x + 1, y] != 0 && _map[x, y + 1] != 0)
                {
                    neighborDir = 1;
                }
                if (_map[x + 1, y] != 0 && _map[x, y - 1] != 0)
                {
                    neighborDir = 2;
                }
            }
            else if (_map[x, y] == 2)
            {
                if (_map[x - 1, y] != 0 && _map[x + 1, y] != 0)
                {
                    neighborDir = 0;
                }

                if (_map[x, y - 1] != 0 && _map[x, y + 1] != 0)
                {
                    neighborDir = 1;
                }
            }
            if (_map[x, y] == 3)
            {
                if (_map[x - 1, y] != 0 && _map[x, y + 1] != 0 && _map[x, y - 1] != 0)
                {
                    neighborDir = 1;
                }
                if (_map[x + 1, y] != 0 && _map[x, y + 1] != 0 && _map[x, y - 1] != 0)
                {
                    neighborDir = 3;
                }
                if (_map[x + 1, y] != 0 && _map[x - 1, y] != 0 && _map[x, y + 1] != 0)
                {
                    neighborDir = 2;
                }
                if (_map[x + 1, y] != 0 && _map[x - 1, y] != 0 && _map[x, y - 1] != 0)
                {
                    neighborDir = 0;
                }
            }
        }
        path.RotateSlot(Vector3.up, _angles[neighborDir]);
    }

    bool isCurved(int x, int y)
    {
        if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
        {
            if (_map[x - 1, y] != 0 && _map[x + 1, y] != 0)
            {
                return false;
            }

            if (_map[x, y - 1] != 0 && _map[x, y + 1] != 0)
            {
                return false;
            }
        }
        return true;
    }
}
