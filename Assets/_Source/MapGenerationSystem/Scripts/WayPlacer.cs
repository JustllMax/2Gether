using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx.Triggers;
using UnityEngine;

public class WayPlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] WayPrefabs;
    [SerializeField] private Way StartingWay;

    private Way[,] _spawnedWays;
    private bool _isWayDone;

    HashSet<Vector2Int> _vacantPlaces = new HashSet<Vector2Int>();

    [SerializeField] private Vector2Int MapSize = new Vector2Int(13, 13);
    [SerializeField] private Vector2Int MapCenter = new Vector2Int(7, 7);


    private int Add(int x, int y) => x + y;

    private int Subtract(int x, int y) => x - y;
    private delegate int Operation(int x, int y);


    // Start is called before the first frame update
    [Obsolete]
    void Start()
    {
        GenerateMap();
    }
    private void GenerateMap()
    {
        _spawnedWays = new Way[MapSize.x, MapSize.y];

        Way centerWay = Instantiate(StartingWay);
        centerWay.pos = MapCenter;

        centerWay.transform.position = new Vector3(centerWay.pos.x, 0, centerWay.pos.y) * 30;

        _spawnedWays[MapCenter.x, MapCenter.y] = StartingWay;

        for (int i = 0; i < 4; i++)
            CheckWay(centerWay, i, 0);
    }
    //[Obsolete]
    private void CheckWay(Way wayToConnect, int i, int w)
    {
        if (wayToConnect.pos.x == 0 || wayToConnect.pos.y == 0 || wayToConnect.pos.x == MapSize.x - 1 || wayToConnect.pos.y == MapSize.y - 1)
            return;

        int id = 1;

        GameObject newWayObject = Instantiate(WayPrefabs[id]);

        Way newWay = newWayObject.GetComponent<Way>();

        newWay.pos = wayToConnect.pos;

        Vector2Int newPos = wayToConnect.pos;

        Debug.Log($"[{i}][{w}] [{wayToConnect.gameObject.GetInstanceID().ToString()}] wayToPlace pos:{wayToConnect.pos.x}; {wayToConnect.pos.y}\nAnchors to connect: {wayToConnect.wayAnchors[0].IsWay}; {wayToConnect.wayAnchors[1].IsWay}; {wayToConnect.wayAnchors[2].IsWay}; {wayToConnect.wayAnchors[3].IsWay}\nAnchors new connect: {newWay.wayAnchors[0].IsWay}; {newWay.wayAnchors[1].IsWay}; {newWay.wayAnchors[2].IsWay}; {newWay.wayAnchors[3].IsWay}");


        if (id == 1)
        {
            if (i == 0)
            {
                if (wayToConnect.wayAnchors[0].IsWay)
                {
                    SetNewWayAnchors(Subtract, true, newPos, wayToConnect, newWay, 2, 0);
                    CheckWay(newWay, i, 0);
                    PlaceOneWay(newWay);
                }
            }
            if (i == 1)
            {
                if (wayToConnect.wayAnchors[1].IsWay)
                {
                    SetNewWayAnchors(Add, false, newPos, wayToConnect, newWay, 3, 1);
                    CheckWay(newWay, i, 1);
                    PlaceOneWay(newWay);
                }
            }
            if (i == 2)
            {
                if (wayToConnect.wayAnchors[2].IsWay)
                {
                    SetNewWayAnchors(Add, true, newPos, wayToConnect, newWay, 3, 2);
                    CheckWay(newWay, i, 2);
                    PlaceOneWay(newWay);
                }
            }
            if (i == 3)
            {
                if (wayToConnect.wayAnchors[3].IsWay)
                {
                    SetNewWayAnchors(Subtract, false, newPos, wayToConnect, newWay, 1, 3);
                    CheckWay(newWay, i, 3);
                    newWay.transform.position = new Vector3(newWay.pos.x, 0, newWay.pos.y) * 30;
                    _spawnedWays[newWay.pos.x, newWay.pos.y] = newWay;
                }
            }
        }
        /*if (id == 2)
        {
            if (wayToConnect.wayAnchors[0].IsWay)
            {
                //TODO Rotation
            }
            if (wayToConnect.wayAnchors[1].IsWay)
            {
                //TODO Rotation
            }
            if (wayToConnect.wayAnchors[2].IsWay)
            {
                x = wayToConnect.pos.x + 1;
                newWay.wayAnchors[0].IsWay = false;
                
                wayToConnect.wayAnchors[2].IsWay = false;

                CheckWay(newWay);
                PlaceOneWay(x, wayToConnect.pos.y, newWay);
            }
            if (wayToConnect.wayAnchors[3].IsWay)
            {
                y = wayToConnect.pos.y - 1;
                newWay.wayAnchors[1].IsWay = false;
                
                wayToConnect.wayAnchors[1].IsWay = false;

                CheckWay(newWay);
                PlaceOneWay(x, wayToConnect.pos.y, newWay);
            }
        }
        if (id == 3)
        {
            if (wayToConnect.wayAnchors[0].IsWay)
            {
                x = wayToConnect.pos.x - 1;

                newWay.wayAnchors[2].IsWay = false;

                wayToConnect.wayAnchors[0].IsWay = false;
            }
            if (wayToConnect.wayAnchors[1].IsWay)
            {
                y = wayToConnect.pos.y + 1;

                newWay.wayAnchors[3].IsWay = false;

                wayToConnect.wayAnchors[1].IsWay = false;
            }
            if (wayToConnect.wayAnchors[2].IsWay)
            {
                x = wayToConnect.pos.x + 1;

                newWay.wayAnchors[3].IsWay = false;
                
                wayToConnect.wayAnchors[2].IsWay = false;
            }
            if (wayToConnect.wayAnchors[3].IsWay)
            {
                y = wayToConnect.pos.y - 1;

                newWay.wayAnchors[3].IsWay = false;
                
                wayToConnect.wayAnchors[1].IsWay = false;
            }
        }
        if (id == 4)
        {
            if (wayToConnect.wayAnchors[0].IsWay)
            {

            }
        }
        */
    }
    private void SetNewWayAnchors(Operation operation, bool axis, Vector2Int newPos, Way wayToConnect, Way newWay, int newWayAnchor, int wayToConnectAnchor)
    {
        if (axis)
            newPos.x = operation(wayToConnect.pos.x, 1);
        else
            newPos.y = operation(wayToConnect.pos.y, 1);

        newWay.wayAnchors[newWayAnchor].IsWay = false;
        wayToConnect.wayAnchors[wayToConnectAnchor].IsWay = false;

        newWay.pos = newPos;
    }
    private void PlaceOneWay(Way wayToPlace)
    {
        wayToPlace.transform.position = new Vector3(wayToPlace.pos.x, 0, wayToPlace.pos.y) * 30;
        Debug.Log($"{wayToPlace.transform.position.x}; {wayToPlace.transform.position.y}; {wayToPlace.transform.position.z};");
        _spawnedWays[wayToPlace.pos.x, wayToPlace.pos.y] = wayToPlace;
    }

    private void PlaceOneEmptyWay()
    {
        for (int x = 0; x < _spawnedWays.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedWays.GetLength(1); y++)
            {
                if (_spawnedWays[x, y] == null) continue;

                int maxX = _spawnedWays.GetLength(0) - 1;
                int maxY = _spawnedWays.GetLength(1) - 1;

                if (x > 0 && _spawnedWays[x - 1, y] == null) _vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _spawnedWays[x, y - 1] == null) _vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && _spawnedWays[x + 1, y] == null) _vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && _spawnedWays[x, y + 1] == null) _vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Way newWay = Instantiate(WayPrefabs[0].GetComponent<Way>());
        Vector2Int position = _vacantPlaces.ElementAt(UnityEngine.Random.Range(0, _vacantPlaces.Count));
        newWay.transform.position = new Vector3(position.x, 0, position.y) * 30;

        _spawnedWays[position.x, position.y] = newWay;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
