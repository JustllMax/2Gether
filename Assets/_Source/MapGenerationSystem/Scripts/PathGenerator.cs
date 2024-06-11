using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Collections;
using UnityEngine;

public class PathGenerator
{
    public int[,] map;
    private Vector2Int[] _directions = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    private Vector2Int _mapSize;
    private Vector2Int _startPos;

    public PathGenerator(Vector2Int mapSize, Vector2Int startPos)
    {
        _mapSize = mapSize;
        _startPos = startPos;
        map = new int[mapSize.x, mapSize.y];
        map[_startPos.x, _startPos.y] = 4;
    }

    public int[,] GenerateMap()
    {
        for (int i = 0; i < 4; i++)
        {
            GeneratePath(i, i);
        }

        return map;
    }
    void GeneratePath(int curDirection, int direction)
    {
        bool isEnd = false;
        Vector2Int pos = _startPos;
        Vector2Int newPos = _startPos;
        while (!isEnd)
        {
            pos = newPos;
            newPos = pos + _directions[curDirection];
            if (pos != _startPos)
                curDirection = GetRandomDirection(direction, newPos);

            if (newPos.x < 0 || newPos.y < 0 || newPos.x > _mapSize.x - 1 || newPos.y > _mapSize.y - 1)
            {
                map[pos.x, pos.y] = 1;
                if (curDirection == 0)
                {
                    map[pos.x + 1, pos.y] += 1;
                }
                else if (direction == 1)
                {
                    map[pos.x, pos.y - 1] += 1;
                }
                else if (direction == 2)
                {
                    map[pos.x - 1, pos.y] += 1;
                }
                else if (direction == 3)
                {
                    map[pos.x, pos.y + 1] += 1;
                }
                isEnd = true;
            }
            if (newPos.x > 0 && newPos.x < _mapSize.x - 1 && newPos.y > 0 && newPos.y < _mapSize.y - 1)
            {
                map[newPos.x, newPos.y] = 1;

                map[newPos.x, newPos.y] = GetNeighborsCount(newPos.x, newPos.y);
                map[pos.x, pos.y] = GetNeighborsCount(pos.x, pos.y);

            }
        }
    }
    private int GetNeighborsCount(int x, int y)
    {
        int count = 0;

        if (x - 1 >= 0 && map[x - 1, y] > 0 && map[x - 1, y] < 6)
            count++;
        if (x + 1 < _mapSize.x && map[x + 1, y] > 0 && map[x + 1, y] < 6)
            count++;
        if (y - 1 >= 0 && map[x, y - 1] > 0)
            count++;
        if (y + 1 < _mapSize.y && map[x, y + 1] > 0 && map[x, y + 1] < 6)
            count++;
        return count;
    }

    int GetRandomDirection(int direction, Vector2Int pos)
    {
        List<int> directions = new List<int>
        {
            direction
        };
        int dir = Random.Range(0, 100);
        if (pos.x > 0 && pos.x < _mapSize.x - 1 && pos.y > 0 && pos.y < _mapSize.y - 1)
        {
            if (direction == 0)
            {
                directions.Add(dir % 2 == 0 ? 0 : 1);
            }
            if (direction == 1)
            {
                directions.Add(dir % 2 == 0 ? 1 : 2);
            }
            if (direction == 2)
            {
                directions.Add(dir % 2 == 0 ? 2 : 3);
            }
            if (direction == 3)
            {
                directions.Add(dir % 2 == 0 ? 3 : 0);
            }
        }

        int rDir = Random.Range(0, directions.Count);

        return directions[rDir];
    }
}
