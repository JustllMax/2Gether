using System.Data;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlotPlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private WaySlot startingSlot;
    [SerializeField] private Vector2Int mapSize = new Vector2Int(13, 13);
    [SerializeField] private Vector2Int startPos = new Vector2Int(7, 7);

    private Vector2Int[] _directions = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    private float[] _angles = { 0f, 90f, 180f, 270f };
    private int[,] _map;
    private Vector2Int newPos;

    void Start()
    {
        GenerateMap();
        PrintMap();
    }

    void GenerateMap()
    {
        _map = new int[mapSize.x, mapSize.y];

        _map[startPos.x, startPos.y] = 4;

        for (int i = 0; i < 4; i++)
        {
            GeneratePath(startPos, i, i);
        }

        int neighborDir = 0;
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (i == startPos.x && j == startPos.y)
                {
                    WaySlot path = Instantiate(startingSlot).GetComponent<WaySlot>();
                    path.gameObject.transform.position = new Vector3(i, 0, j) * 30;
                    path.slot.pos = new Vector2Int(i, j);
                }
                else
                {
                    WaySlot path;
                    if (_map[i, j] == 2)
                    {
                        if (isCurved(i, j))
                            path = Instantiate(slotPrefabs[5]).GetComponent<WaySlot>();
                        else
                            path = Instantiate(slotPrefabs[2]).GetComponent<WaySlot>();
                    }

                    else
                        path = Instantiate(slotPrefabs[_map[i, j]]).GetComponent<WaySlot>();

                    path.gameObject.transform.position = new Vector3(i, 0, j) * 30;
                    path.slot.pos = new Vector2Int(i, j);

                    if (HasNeighbors(i, j, out neighborDir))
                    {
                        //if (_map[i, j] == 2)
                        {
                            if (neighborDir == 0)
                            {

                            }
                            if (neighborDir == 1)
                            {

                            }
                            if (neighborDir == 2)
                            {

                            }
                            if (neighborDir == 3)
                            {

                            }
                        }
                        path.RotateSlot(Vector3.up, _angles[neighborDir]);
                    }
                }
            }
        }
    }


    void GeneratePath(Vector2Int pos, int curDirection, int direction)
    {
        Vector2Int newPos = pos + _directions[curDirection];
        curDirection = GetRandomDirection(direction, newPos);
        if (newPos.x >= 0 && newPos.x < mapSize.x && newPos.y >= 0 && newPos.y < mapSize.y)
        {

            if (newPos.x < 0 || newPos.y < 0 || newPos.x >= mapSize.x || newPos.y >= mapSize.y)
            {
                _map[newPos.x, newPos.y] = 1;
                return;
            }

            _map[newPos.x, newPos.y] = GetNeighborsCount(newPos.x, newPos.y);
            _map[pos.x, pos.y] = GetNeighborsCount(pos.x, pos.y);

            Debug.Log($"Pos:[{pos.x},{pos.y}] new:[{newPos.x},{newPos.y}] dir:{curDirection} value:{_map[pos.x, pos.y]}");
            GeneratePath(newPos, curDirection, direction);
        }
        else
            return;
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
    bool HasNeighbors(int x, int y, out int neighborDir)
    {
        int numRows = _map.GetLength(0);
        int numCols = _map.GetLength(1);
        if (x > 0 && _map[x - 1, y] != 0)
        {
            neighborDir = 0;
            return true;
        }
        if (x < numRows - 1 && _map[x + 1, y] != 0)
        {
            neighborDir = 1;
            return true;
        }

        if (y > 0 && _map[x, y - 1] != 0)
        {
            neighborDir = 2;
            return true;
        }
        if (y < numCols - 1 && _map[x, y + 1] != 0)
        {
            neighborDir = 3;
            return true;
        }
        neighborDir = -1;
        return false;
    }

    private int GetNeighborsCount(int x, int y)
    {
        int count = 0;

        if (x - 1 >= 0 && _map[x - 1, y] > 0) // Влево
            count++;
        if (x + 1 < mapSize.x && _map[x + 1, y] > 0) // Вправо
            count++;
        if (y - 1 >= 0 && _map[x, y - 1] > 0) // Вниз
            count++;
        if (y + 1 < mapSize.y && _map[x, y + 1] > 0) // Вверх
            count++;

        return count;
    }

    int GetRandomDirection(int direction, Vector2Int pos)
    {
        //(direction + 2) % 4
        int randomDirection = Random.Range(0, 4);
        Vector2Int newPos = pos + _directions[randomDirection];
        //int neighborDirection = 0;
        bool flag = false;
        if (randomDirection == (direction + 2) % 4)
            return direction;
        while (flag == false)
        {
            if (newPos.x >= 0 && newPos.x < mapSize.x && newPos.y >= 0 && newPos.y < newPos.y)
            {
                if (direction == 0)
                {
                    if ((newPos.x >= 0 && newPos.x < startPos.x && newPos.y >= 0 && newPos.y < startPos.y) &&
                        (_map[newPos.x, newPos.y] > 0))
                    {
                        flag = true;
                    }
                }
                if (direction == 1)
                {
                    if (newPos.x >= startPos.x && newPos.x < mapSize.x && newPos.y >= 0 && newPos.y < startPos.y &&
                        (_map[newPos.x, newPos.y] > 0))
                    {
                        flag = true;
                    }
                }
                if (direction == 2)
                {
                    if (newPos.x >= startPos.x && newPos.x < mapSize.x && newPos.y >= startPos.y && newPos.y < mapSize.y &&
                        (_map[newPos.x, newPos.y] > 0))
                    {
                        flag = true;
                    }
                }
                if (direction == 3)
                {
                    if (newPos.x >= 0 && newPos.x < startPos.x && newPos.y >= startPos.y && newPos.y < mapSize.y &&
                        (_map[newPos.x, newPos.y] > 0))
                    {
                        flag = true;
                    }
                }
            }
            ++randomDirection;
            newPos = pos + _directions[randomDirection];
        }

        if(flag)
        {
            return randomDirection;
        }
        return direction;
    }



    void PrintMap()
    {
        string line = "";
        for (int y = 0; y < mapSize.x; y++)
        {
            for (int x = 0; x < mapSize.y; x++)
            {
                line += _map[x, y] + "\t";
            }
            line += "\n\n";
        }
        Debug.Log(line);
    }
}
