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

    [SerializeField] private GameObject[] _slotPrefabs;
    [SerializeField] private GameObject[] _emptySlotPrefabs;
    [SerializeField] private GameObject _mapBarrier;
    [SerializeField] private GameObject _mapBarrierCorener;
    [SerializeField] private PathSlot _startingSlot;
    #endregion

    #region map

    [SerializeField] private List<MapObject> _maps;
    [SerializeField] private int _mapId;
    [SerializeField] private bool _isRandomGeneration = false;
    [Header("Decoration Spawn Ration in Percent")]
    [SerializeField] private int _decorationSpawnRation;
    #endregion

    #region private variables

    private float[] _angles = { 0f, 90f, 180f, 270f };
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private Vector2Int _startPos;
    public Vector2Int startPos
    {
        get { return _startPos; }
    }
    private int[,] _map;
    #endregion

    #region Action
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
        if (!_isRandomGeneration)
        {
            _mapSize = _maps[_mapId].mapSize;
            _startPos = _maps[_mapId].startPos;
        }
        Generate();

        Debug.Log("SlotPlacer Invoke");
        OnMapGenerated?.Invoke();
    }

    void Generate()
    {
        _spawnedSlots = new PathSlot[_mapSize.x, _mapSize.y];
        if (_isRandomGeneration)
        {
            PathGenerator pathGenerator = new PathGenerator(_mapSize, _startPos);
            pathGenerator.GenerateMap();
            _map = (int[,])pathGenerator.map.Clone();
        }
        else
            _map = (int[,])_maps[_mapId].map.Clone();

        GenerateMap();
        GenerateBarriers();
    }

    void GenerateBarriers()
    {
        //Top
        int i = 0;
        i = 0;
        GameObject barrierCorner = Instantiate(_mapBarrierCorener);
        barrierCorner.transform.position = new Vector3(i * 30 + 10, 0, _mapSize.y * 30 - 5f);
        barrierCorner.transform.rotation = Quaternion.Euler(0, 0, 0);
        barrierCorner.transform.SetParent(this.transform);
        i++;
        for (; i < _mapSize.x; i++)
        {
            GameObject barrier = Instantiate(_mapBarrier);
            barrier.transform.position = new Vector3(i * 30 + 10, 0, _mapSize.y * 30 - 5f);
            barrier.transform.rotation = Quaternion.Euler(0, 0, 0);
            barrier.transform.SetParent(this.transform);
        }


        //Bottom
        i = _mapSize.x - 1;
        barrierCorner = Instantiate(_mapBarrierCorener);
        barrierCorner.transform.position = new Vector3(i * 30 + 10, 0, 0 * 30 - 5f);
        barrierCorner.transform.rotation = Quaternion.Euler(0, 180, 0);
        barrierCorner.transform.SetParent(this.transform);
        i--;
        for (; i >= 0; i--)
        {
            GameObject barrier = Instantiate(_mapBarrier);
            barrier.transform.position = new Vector3(i * 30 + 10, 0, 0 * 30 - 5f);
            barrier.transform.rotation = Quaternion.Euler(0, 180, 0);
            barrier.transform.SetParent(this.transform);
        }

        //Left
        int j = 0;
        barrierCorner = Instantiate(_mapBarrierCorener);
        barrierCorner.transform.position = new Vector3(0 * 30 - 5f, 0, j * 30 + 10f);
        barrierCorner.transform.rotation = Quaternion.Euler(0, -90, 0);
        barrierCorner.transform.SetParent(this.transform);
        j++;
        for (; j < _mapSize.y; j++)
        {
            GameObject barrier = Instantiate(_mapBarrier);
            barrier.transform.position = new Vector3(0 * 30 - 5f, 0, j * 30 + 10f);
            barrier.transform.rotation = Quaternion.Euler(0, -90, 0);
            barrier.transform.SetParent(this.transform);
        }

        //Right
        j = _mapSize.y - 1;
        barrierCorner = Instantiate(_mapBarrierCorener);
        barrierCorner.transform.position = new Vector3(_mapSize.x * 30 - 5f, 0, j * 30 + 10f);
        barrierCorner.transform.rotation = Quaternion.Euler(0, 90, 0);
        barrierCorner.transform.SetParent(this.transform);
        j--;
        for (; j >= 0; j--)
        {
            GameObject barrier = Instantiate(_mapBarrier);
            barrier.transform.position = new Vector3(_mapSize.x * 30 - 5f, 0, j * 30 + 10f);
            barrier.transform.rotation = Quaternion.Euler(0, 90, 0);
            barrier.transform.SetParent(this.transform);
        }
    }

    void GenerateMap()
    {
        int i = 0;
        PathSlot path = new();
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                if (y == _startPos.x && x == _startPos.y)
                {
                    path = Instantiate(_startingSlot).GetComponent<PathSlot>();
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
                            path = Instantiate(_slotPrefabs[5]).GetComponent<PathSlot>();
                        else
                            path = Instantiate(_slotPrefabs[2]).GetComponent<PathSlot>();
                    }
                    else if (_map[x, y] == 0 || _map[x, y] > 5)
                    {
                        if (!_isRandomGeneration && _map[x, y] > 5)
                        {
                            path = Instantiate(_emptySlotPrefabs[_map[x, y] - 5]).GetComponent<PathSlot>();
                        }
                        else if (_isRandomGeneration)
                        {
                            if (_emptySlotPrefabs.Length > 1)
                            {
                                int randEmptySlot = Random.Range(1, 100);
                                path = Instantiate(_emptySlotPrefabs[randEmptySlot > _decorationSpawnRation ? 0 : Random.Range(1, _emptySlotPrefabs.Length)]).GetComponent<PathSlot>();
                            }
                        }
                        else
                            path = Instantiate(_emptySlotPrefabs[0]).GetComponent<PathSlot>();
                    }
                    else
                    {
                        path = Instantiate(_slotPrefabs[_map[x, y]]).GetComponent<PathSlot>();
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
        if (x >= 0 && x < _mapSize.x && y >= 0 && y < _mapSize.y)
        {
            if (_map[x, y] == 1)
            {
                if (y == 0 && isHaveNeighbor(_map[x, y + 1]))
                {
                    neighborDir = 1;
                }
                if (y == _mapSize.y - 1 && isHaveNeighbor(_map[x, y - 1]))
                {
                    neighborDir = 3;
                }

                if (x == _mapSize.x - 1 && isHaveNeighbor(_map[x - 1, y]))
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
        if (val > 0 && val < _slotPrefabs.Length)
            return true;
        else
            return false;
    }
    bool isCurved(int x, int y)
    {
        if (x > 0 && x < _mapSize.x - 1 && y > 0 && y < _mapSize.y - 1)
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
