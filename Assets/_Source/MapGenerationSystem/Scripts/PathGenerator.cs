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
        while (!isEnd)
        {
            Vector2Int newPos = pos + _directions[direction];
            if (pos != _startPos)
                curDirection = GetRandomDirection(direction, curDirection, newPos);

            if (newPos.x >= 0 && newPos.x < _mapSize.x && newPos.y >= 0 && newPos.y < _mapSize.y)
            {
                //Debug.Log($"Pos:[{pos.x},{pos.y}] new:[{newPos.x},{newPos.y}] dir:{direction} randDir:{curDirection} value:{map[pos.x, pos.y]}");
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= _mapSize.x || newPos.y >= _mapSize.y)
                {
                    map[newPos.x, newPos.y] = 1;
                    isEnd = true;
                }

                map[newPos.x, newPos.y] = GetNeighborsCount(newPos.x, newPos.y);
                map[pos.x, pos.y] = GetNeighborsCount(pos.x, pos.y);



            }

            /*
            Vector2Int newPos = pos + _directions[curDirection];

            if (pos != _startPos)
                curDirection = GetRandomDirection(direction, curDirection, newPos);

            if (newPos.x >= 0 && newPos.x < _mapSize.x && newPos.y >= 0 && newPos.y < _mapSize.y)
            {
                //Debug.Log($"Pos:[{pos.x},{pos.y}] new:[{newPos.x},{newPos.y}] dir:{direction} randDir:{curDirection} value:{map[pos.x, pos.y]}");
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= _mapSize.x || newPos.y >= _mapSize.y)
                {
                    map[newPos.x, newPos.y] = 1;
                    return;
                }

                map[newPos.x, newPos.y] = GetNeighborsCount(newPos.x, newPos.y);
                map[pos.x, pos.y] = GetNeighborsCount(pos.x, pos.y);

                GeneratePath(newPos, curDirection, direction);
            }
            else
                return;
            */
        }
    }
    private int GetNeighborsCount(int x, int y)
    {
        int count = 0;

        if (x - 1 >= 0 && map[x - 1, y] > 0) // Влево
            count++;
        if (x + 1 < _mapSize.x && map[x + 1, y] > 0) // Вправо
            count++;
        if (y - 1 >= 0 && map[x, y - 1] > 0) // Вниз
            count++;
        if (y + 1 < _mapSize.y && map[x, y + 1] > 0) // Вверх
            count++;
        return count;
    }

    int GetRandomDirection(int direction, int curDirection, Vector2Int pos)
    {
        //(direction + 2) % 4
        Vector2Int newPos = new Vector2Int();
        List<int> directions = new List<int>();

        directions.Add(direction);

        for (int i = 0; i < 4; i++)
        {
            newPos = pos + _directions[i];

            if (newPos.x >= 0 && newPos.x < _mapSize.x && newPos.y >= 0 && newPos.y < _mapSize.y)
            {
                bool isDirection = true;
                if (direction == 0)
                {
                    if (
                        newPos.x >= 0 && newPos.x < _startPos.x &&
                        newPos.y >= _startPos.y && newPos.y <= _startPos.y &&
                        HasNeighbors(newPos.x, newPos.y, out curDirection) && curDirection != direction
                        //isDirection(pos.x, pos.y, direction, curDirection, i)
                        )
                    {

                        isDirection = false;
                    }
                }
                if (direction == 1)
                {
                    if (
                        newPos.x >= _startPos.x && newPos.x < _mapSize.x &&
                        newPos.y > _startPos.y && newPos.y < _mapSize.y &&
                        HasNeighbors(newPos.x, newPos.y, out curDirection) && curDirection != direction
                        //isDirection(pos.x, pos.y, direction, curDirection, i)
                        )
                    {
                        isDirection = false;
                    }
                }
                if (direction == 2)
                {
                    if (
                        newPos.x > _startPos.x && newPos.x < _mapSize.x &&
                        newPos.y >= 0 && newPos.y <= _startPos.y &&
                        HasNeighbors(newPos.x, newPos.y, out curDirection) && curDirection != direction
                        //isDirection(pos.x, pos.y, direction, curDirection, i)
                        )
                    {
                        isDirection = false;
                    }
                }
                if (direction == 3)
                {
                    if (
                        newPos.x >= 0 && newPos.x <= _startPos.x &&
                        newPos.y >= 0 && newPos.y < _startPos.y &&
                        HasNeighbors(newPos.x, newPos.y, out curDirection) && curDirection != direction
                        //isDirection(pos.x, pos.y, direction, curDirection, i)
                        )
                    {
                        isDirection = false;
                    }
                }
                if (isDirection)
                    directions.Add(i);
            }
            else
            {
                return direction;
            }

        }
        return directions[Random.Range(0, directions.Count)];
    }
    bool isDirection(int x, int y, int direction, int curDirection, int i)
    {
        if (curDirection == i)
        {
            if (curDirection == 0)
            {
                if (map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 0)
                {

                }
            }
            if (curDirection == 1)
            {
                if (map[x - 1, y] == 0 && map[x + 1, y] == 0 && map[x, y + 1] == 0)
                {

                }
            }
            if (curDirection == 2)
            {
                if (map[x, y + 1] == 0 && map[x + 1, y] == 0 && map[x, y - 1] == 0)
                {

                }
            }
            if (curDirection == 3)
            {
                if (map[x - 1, y] == 0 && map[x + 1, y] == 0 && map[x, y - 1] == 0)
                {

                }
            }
        }
        else
        {
            if (curDirection == 0)
            {
                if (map[x - 1, y] == 0 && map[x, y + 1] == 0 && map[x, y - 1] == 0)
                {

                }
            }
            if (curDirection == 1)
            {
                if (map[x - 1, y] == 0 && map[x + 1, y] == 0 && map[x, y + 1] == 0)
                {

                }
            }
            if (curDirection == 2)
            {
                if (map[x, y + 1] == 0 && map[x + 1, y] == 0 && map[x, y - 1] == 0)
                {

                }
            }
            if (curDirection == 3)
            {
                if (map[x - 1, y] == 0 && map[x + 1, y] == 0 && map[x, y - 1] == 0)
                {

                }
            }
        }

        if (map[x - 1, y - 1] == 0)
        {
            /*
            0 0 0
            0 1 0
            1 0 0
            */
        }
        if (map[x - 1, y + 1] == 0)
        {
            /*
            1 0 0
            0 1 0
            0 0 0
            */
        }
        if (map[x + 1, y + 1] == 0)
        {
            /*
            0 0 1
            0 1 0
            0 0 0
            */
        }
        if (map[x + 1, y - 1] == 0)
        {
            /*
            0 0 0
            0 1 0
            0 0 1
            */
        }
        return true;
    }
    bool HasNeighbors(int x, int y, out int neighborDir)
    {
        int numRows = map.GetLength(0);
        int numCols = map.GetLength(1);
        if (x > 0 && map[x - 1, y] != 0)
        {
            neighborDir = 0;
            return true;
        }
        if (x < numRows - 1 && map[x + 1, y] != 0)
        {
            neighborDir = 1;
            return true;
        }

        if (y > 0 && map[x, y - 1] != 0)
        {
            neighborDir = 2;
            return true;
        }
        if (y < numCols - 1 && map[x, y + 1] != 0)
        {
            neighborDir = 3;
            return true;
        }
        neighborDir = -1;
        return false;
    }
}
