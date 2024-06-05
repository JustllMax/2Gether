using System;
using System.Collections.Generic;
using System.Threading;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;


public class SlotPlacer : MonoBehaviour
{

    private static SlotPlacer _instance;
    public static SlotPlacer Instance { get { return _instance; } }

    #region slots
    private PathSlot[,] _spawnedSlots;
    public PathSlot[,] spawnedSlots
    {
        get { return _spawnedSlots; }
        set { _spawnedSlots = value; }
    }
    private List<PathSlot> _spawnSlots = new List<PathSlot>();
    public List<PathSlot> spawnSlots
    {
        get { return _spawnSlots; }
        set { _spawnSlots = value; }
    }

    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private GameObject[] emptySlotPrefabs;
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
    private Vector2Int _startPos;
    public Vector2Int startPos
    {
        get { return _startPos; }
    }
    private int[,] _map;
    #endregion

    #region NavMeshVariables
    public delegate void SlotPlacerGeneratedHandler();
    public static event Action OnMapGenerated;

    #endregion

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    void Start()
    {
        mapSize = maps[mapId].mapSize;
        _startPos = maps[mapId].startPos;
        Generate();

        Debug.Log("SlotPlacer Invoke");
        OnMapGenerated?.Invoke();
    }

    void Generate()
    {
        _spawnedSlots = new PathSlot[mapSize.x, mapSize.y];
        if (isRandomGeneration)
        {
            PathGenerator pathGenerator = new PathGenerator(mapSize, _startPos);
            pathGenerator.GenerateMap();
            _map = (int[,])pathGenerator.map.Clone();
        }
        else
            _map = (int[,])maps[mapId].map.Clone();

        GenerateMap();
    }

    void GenerateMap()
    {
        int i = 0;
        PathSlot path = new();
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                if (y == _startPos.x && x == _startPos.y)
                {
                    path = Instantiate(startingSlot).GetComponent<PathSlot>();
                    path.gameObject.transform.position = new Vector3(x, 0, y) * 30;
                    path.slot.pos = new Vector2Int(x, y);
                    _spawnedSlots[x, y] = path;
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
                    else if (_map[x, y] == 0 || _map[x, y] > 5)
                    {
                        if (!isRandomGeneration && _map[x, y] > 5)
                        {
                            path = Instantiate(emptySlotPrefabs[_map[x, y] - 5]).GetComponent<PathSlot>();
                        }
                        else if (isRandomGeneration)
                        {
                            if (emptySlotPrefabs.Length > 1)
                            {
                                int randEmptySlot = Random.Range(0, 100);
                                path = Instantiate(emptySlotPrefabs[randEmptySlot > 10 ? 0 : Random.Range(1, emptySlotPrefabs.Length)]).GetComponent<PathSlot>();
                            }
                        }
                        else
                            path = Instantiate(emptySlotPrefabs[0]).GetComponent<PathSlot>();
                    }
                    else
                    {
                        path = Instantiate(slotPrefabs[_map[x, y]]).GetComponent<PathSlot>();
                    }

                    path.gameObject.transform.position = new Vector3(x, 0, y) * 30;
                    path.slot.pos = new Vector2Int(x, y);
                    path.slot.value = _map[x, y];

                    #region parent   

                    if (path.slot.value == 1)
                    {
                        path.name = "spawn " + i++;
                        path.gameObject.transform.SetParent(this.transform.GetChild(1));

                        _spawnSlots.Add(path);
                    }
                    else
                    {
                        path.gameObject.transform.SetParent(this.transform.GetChild(0));
                    }
                    #endregion
                }
                RotatePath(x, y, path);
                _spawnedSlots[x, y] = path;
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
                if (y == 0 && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 1;
                }
                if (y == mapSize.y - 1 && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 3;
                }

                if (x == mapSize.x - 1 && isHaveNeighbor(_map[x - 1, y]))
                {
                    neighborDir = 0;
                }
                if (x == 0 && isHaveNeighbor(_map[x + 1, y]))
                {
                    neighborDir = 2;
                }
            }
            if (_map[x, y] == 2 && isCurved(x, y))
            {
                if (isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 3;
                }
                if (isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 0;
                }
                if (isHaveNeighbor(_map[x + 1, y]) && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 1;
                }
                if (isHaveNeighbor(_map[x + 1, y]) && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 2;
                }
            }
            else if (_map[x, y] == 2)
            {
                if (isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x + 1, y]))
                {
                    neighborDir = 0;
                }

                if (isHaveNeighbor(_map[x, y - 1]) && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 1;
                }
            }
            if (_map[x, y] == 3)
            {
                if (isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x, y + 1]) && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 1;
                }
                if (isHaveNeighbor(_map[x + 1, y]) && isHaveNeighbor(_map[x, y + 1]) && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 3;
                }
                if (isHaveNeighbor(_map[x + 1, y]) && isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 2;
                }
                if (isHaveNeighbor(_map[x + 1, y]) && isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 0;
                }
            }
        }
        path.RotateSlot(Vector3.up, _angles[neighborDir]);
    }
    private bool isHaveNeighbor(int val)
    {
        if (val != 0 && val < 6)
            return true;
        else
            return false;
    }
    bool isCurved(int x, int y)
    {
        if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
        {
            if (isHaveNeighbor(_map[x - 1, y]) && isHaveNeighbor(_map[x + 1, y])
            ||
                isHaveNeighbor(_map[x, y - 1]) && isHaveNeighbor(_map[x, y + 1]))
            {
                return false;
            }

        }


        return true;
    }
}
