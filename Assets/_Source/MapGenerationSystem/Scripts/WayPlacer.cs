using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx.Triggers;
using UnityEngine;

public class SlotPlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] SlotPrefabs;
    [SerializeField] private Slot StartingSlot;

    private Slot[,] _spawnedSlots;
    private bool _isSlotDone;

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
        _spawnedSlots = new Slot[MapSize.x, MapSize.y];

        Slot centerSlot = Instantiate(StartingSlot);
        centerSlot.pos = MapCenter;

        centerSlot.transform.position = new Vector3(centerSlot.pos.x, 0, centerSlot.pos.y) * 30;

        _spawnedSlots[MapCenter.x, MapCenter.y] = StartingSlot;

        for (int i = 0; i < 4; i++)
            CheckSlot(centerSlot, i, 0);

    }
    //[Obsolete]
    private void CheckSlot(Slot SlotToConnect, int i, int w)
    {
        if (SlotToConnect.pos.x == 0 || SlotToConnect.pos.y == 0 || SlotToConnect.pos.x == MapSize.x - 1 || SlotToConnect.pos.y == MapSize.y - 1)
            return;

        int id = (UnityEngine.Random.Range(0, 20) == 0) ? 4 : 1;
        //id = 4;

        Slot newSlot = Instantiate(SlotPrefabs[id]).GetComponent<Slot>();

        newSlot.pos = SlotToConnect.pos;

        Vector2Int newPos = SlotToConnect.pos;

        //Debug.Log($"[{i}][{w}] [{SlotToConnect.gameObject.GetInstanceID().ToString()}] SlotToPlace pos:{SlotToConnect.pos.x}; {SlotToConnect.pos.y}\nAnchors to connect: {SlotToConnect.SlotAnchors[0].IsSlot}; {SlotToConnect.SlotAnchors[1].IsSlot}; {SlotToConnect.SlotAnchors[2].IsSlot}; {SlotToConnect.SlotAnchors[3].IsSlot}\nAnchors new connect: {newSlot.SlotAnchors[0].IsSlot}; {newSlot.SlotAnchors[1].IsSlot}; {newSlot.SlotAnchors[2].IsSlot}; {newSlot.SlotAnchors[3].IsSlot}");


        if (id == 1)
        {
            if (i == 0)
            {
                if (SlotToConnect.slotAnchors[0].IsWay)
                {
                    SetNewSlotAnchors(Subtract, true, newPos, SlotToConnect, newSlot, 2, 0);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    CheckSlot(newSlot, i, 0);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 1)
            {
                if (SlotToConnect.slotAnchors[1].IsWay)
                {
                    newSlot.RotateWay(Vector3.up, 90f);
                    SetNewSlotAnchors(Add, false, newPos, SlotToConnect, newSlot, 3, 1);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    CheckSlot(newSlot, i, 1);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 2)
            {
                if (SlotToConnect.slotAnchors[2].IsWay)
                {
                    SetNewSlotAnchors(Add, true, newPos, SlotToConnect, newSlot, 0, 2);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    CheckSlot(newSlot, i, 2);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 3)
            {
                if (SlotToConnect.slotAnchors[3].IsWay)
                {
                    newSlot.RotateWay(Vector3.up, 90f);
                    SetNewSlotAnchors(Subtract, false, newPos, SlotToConnect, newSlot, 1, 3);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    CheckSlot(newSlot, i, 3);
                    PlaceOneSlot(newSlot);
                }
            }
        }
        /*if (id == 2)
        {
            if (SlotToConnect.SlotAnchors[0].IsSlot)
            {
                //TODO Rotation
            }
            if (SlotToConnect.SlotAnchors[1].IsSlot)
            {
                //TODO Rotation
            }
            if (SlotToConnect.SlotAnchors[2].IsSlot)
            {
                x = SlotToConnect.pos.x + 1;
                newSlot.SlotAnchors[0].IsSlot = false;
                
                SlotToConnect.SlotAnchors[2].IsSlot = false;

                CheckSlot(newSlot);
                PlaceOneSlot(x, SlotToConnect.pos.y, newSlot);
            }
            if (SlotToConnect.SlotAnchors[3].IsSlot)
            {
                y = SlotToConnect.pos.y - 1;
                newSlot.SlotAnchors[1].IsSlot = false;
                
                SlotToConnect.SlotAnchors[1].IsSlot = false;

                CheckSlot(newSlot);
                PlaceOneSlot(x, SlotToConnect.pos.y, newSlot);
            }
        }
        /*if (id == 3)
        {
            if (SlotToConnect.SlotAnchors[0].IsSlot)
            {
                x = SlotToConnect.pos.x - 1;

                newSlot.SlotAnchors[2].IsSlot = false;

                SlotToConnect.SlotAnchors[0].IsSlot = false;
            }
            if (SlotToConnect.SlotAnchors[1].IsSlot)
            {
                y = SlotToConnect.pos.y + 1;

                newSlot.SlotAnchors[3].IsSlot = false;

                SlotToConnect.SlotAnchors[1].IsSlot = false;
            }
            if (SlotToConnect.SlotAnchors[2].IsSlot)
            {
                x = SlotToConnect.pos.x + 1;

                newSlot.SlotAnchors[3].IsSlot = false;
                
                SlotToConnect.SlotAnchors[2].IsSlot = false;
            }
            if (SlotToConnect.SlotAnchors[3].IsSlot)
            {
                y = SlotToConnect.pos.y - 1;

                newSlot.SlotAnchors[3].IsSlot = false;
                
                SlotToConnect.SlotAnchors[1].IsSlot = false;
            }
        }
        */
        if (id == 4)
        {
            if (i == 0)
            {
                if (SlotToConnect.slotAnchors[0].IsWay)
                {
                    SetNewSlotAnchors(Subtract, true, newPos, SlotToConnect, newSlot, 2, 0);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    for (int j = 0; j < 4; j++)
                        CheckSlot(newSlot, j, 0);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 1)
            {
                if (SlotToConnect.slotAnchors[1].IsWay)
                {
                    SetNewSlotAnchors(Add, false, newPos, SlotToConnect, newSlot, 3, 1);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    for (int j = 0; j < 4; j++)
                        CheckSlot(newSlot, j, 1);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 2)
            {
                if (SlotToConnect.slotAnchors[2].IsWay)
                {
                    SetNewSlotAnchors(Add, true, newPos, SlotToConnect, newSlot, 0, 2);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    for (int j = 0; j < 4; j++)
                        CheckSlot(newSlot, j, 2);
                    PlaceOneSlot(newSlot);
                }
            }
            if (i == 3)
            {
                if (SlotToConnect.slotAnchors[3].IsWay)
                {
                    SetNewSlotAnchors(Subtract, false, newPos, SlotToConnect, newSlot, 1, 3);
                    if (_spawnedSlots[newSlot.pos.x, newSlot.pos.y] != null) return;
                    for (int j = 0; j < 4; j++)
                        CheckSlot(newSlot, j, 3);
                    PlaceOneSlot(newSlot);
                }
            }
        }

    }
    // axis true = x
    // axis false = z
    private void SetNewSlotAnchors(Operation operation, bool axis, Vector2Int newPos, Slot SlotToConnect, Slot newSlot, int newSlotAnchor, int SlotToConnectAnchor)
    {
        Debug.Log($"[1]new pos x:{newPos.x} y:{newPos.y}");
        if (axis == true)
            newPos.x = operation(SlotToConnect.pos.x, 1);
        if (axis == false)
            newPos.y = operation(SlotToConnect.pos.y, 1);
        Debug.Log($"[2]new pos x:{newPos.x} y:{newPos.y}");

        newSlot.slotAnchors[newSlotAnchor].IsWay = false;
        SlotToConnect.slotAnchors[SlotToConnectAnchor].IsWay = false;

        newSlot.pos = newPos;
    }
    private void PlaceOneSlot(Slot SlotToPlace)
    {

        SlotToPlace.transform.position = new Vector3(SlotToPlace.pos.x, 0, SlotToPlace.pos.y) * 30;
        //Debug.Log($"{SlotToPlace.transform.position.x}; {SlotToPlace.transform.position.y}; {SlotToPlace.transform.position.z};");
        _spawnedSlots[SlotToPlace.pos.x, SlotToPlace.pos.y] = SlotToPlace;
    }

    private void PlaceOneEmptySlot()
    {
        for (int x = 0; x < _spawnedSlots.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedSlots.GetLength(1); y++)
            {
                if (_spawnedSlots[x, y] == null) continue;

                Slot newSlot = Instantiate(SlotPrefabs[0].GetComponent<Slot>());
                newSlot.transform.position = new Vector3(newSlot.pos.x, 0, newSlot.pos.y) * 30;
                _spawnedSlots[newSlot.pos.x, newSlot.pos.y] = newSlot;
            }
        }
        Vector2Int position = _vacantPlaces.ElementAt(UnityEngine.Random.Range(0, _vacantPlaces.Count));
    }

    void Update()
    {

    }
}
