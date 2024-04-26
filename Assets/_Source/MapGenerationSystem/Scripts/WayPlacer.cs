using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Rendering;

public class SlotPlacer : MonoBehaviour
{
    [SerializeField] private GameObject[] SlotPrefabs;
    [SerializeField] private WaySlot StartingSlot;

    private Vector2Int[] directions = { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
    private float[] angles = { 0f, 90f, 180f, 270f };

    private WaySlot[,] _spawnedSlots;

    HashSet<Vector2Int> _vacantPlaces = new HashSet<Vector2Int>();

    [SerializeField] private Vector2Int MapSize = new Vector2Int(13, 13);
    [SerializeField] private Vector2Int MapCenter = new Vector2Int(7, 7);


    private int Add(int x, int y) => x + y;
    private int Subtract(int x, int y) => x - y;
    private delegate int Operation(int x, int y);

    private void Start()
    {
        GenerateMap();
        //yield return new WaitForSecondsRealtime(0.5f);
    }

    private void GenerateMap()
    {
        _spawnedSlots = new WaySlot[MapSize.x, MapSize.y];

        WaySlot centerSlot = Instantiate(StartingSlot);
        centerSlot.slot.pos = MapCenter;
        centerSlot.transform.position = new Vector3(centerSlot.slot.pos.x, 0, centerSlot.slot.pos.y) * 30;

        _spawnedSlots[MapCenter.x, MapCenter.y] = StartingSlot;

        for (int i = 0; i < 4; i++)
            CheckSlot(centerSlot, i, i);
    }

    private void CheckSlot(WaySlot slotToConnect, int direction, int lastDirection)
    {
        int reverseDirection = (direction + 2) % 4;

        Vector2Int newPos = slotToConnect.slot.pos + directions[direction];

        if (newPos.x < 1 || newPos.x >= MapSize.x - 1 || newPos.y < 1 || newPos.y >= MapSize.y - 1)
        {
            WaySlot lastSlot = Instantiate(SlotPrefabs[5]).GetComponent<WaySlot>();

            lastSlot.RotateSlot(Vector3.up, angles[(direction + 2) % 4]);

            SetSlot(newPos, ref lastSlot, ref slotToConnect, direction, reverseDirection);
            return;
        }

        if (_spawnedSlots[newPos.x, newPos.y] != null)
        {
            return;
        }

        int randomId = RandomId();

        WaySlot newSlot = Instantiate(SlotPrefabs[randomId]).GetComponent<WaySlot>();

        if (randomId == 1)
        {
            newSlot.RotateSlot(Vector3.up, angles[direction]);
            if (newSlot.slotAnchors[direction])
            {
                SetSlot(newPos, ref newSlot, ref slotToConnect, reverseDirection, direction);
                CheckSlot(newSlot, direction, direction);
            }
            //Debug.Log(@$"direction:{direction} id:{newSlot.slot.id} x:{newPos.x} z:{newPos.y} angle:{angles[direction]} 
            //{newSlot.slotAnchors[0]}; {newSlot.slotAnchors[1]}; {newSlot.slotAnchors[2]}; {newSlot.slotAnchors[3]}");
        }

        if (randomId == 2)
        {
            int newDirection = CheckAnchors(newPos, newSlot, slotToConnect, direction, lastDirection);

            Debug.Log($"newDir:{newDirection}, dir:{direction}, reverseDir::{reverseDirection}");
            if (newSlot.slotAnchors[newDirection])
            {
                SetSlot(newPos, ref newSlot, ref slotToConnect, reverseDirection, direction);
                CheckSlot(newSlot, newDirection, direction);
            }


        }
        Debug.Log(@$"direction:{direction} id:{newSlot.slot.id} x:{newPos.x} z:{newPos.y} angle:{angles[direction]} 
        {newSlot.slotAnchors[0]}; {newSlot.slotAnchors[1]}; {newSlot.slotAnchors[2]}; {newSlot.slotAnchors[3]}");

        {
            //newSlot.RotateSlot(Vector3.up, angles[direction]);
            if (newSlot.slotAnchors[direction])
            {
                SetSlot(newPos, ref newSlot, ref slotToConnect, direction, reverseDirection);

                _spawnedSlots[newPos.x, newPos.y] = newSlot;
                for (int i = 0; i < 4; i++)
                    CheckSlot(newSlot, i, i);
            }
            Debug.Log(@$"direction:{direction} id:{newSlot.slot.id} x:{newPos.x} z:{newPos.y} angle:{angles[direction]} 
            {newSlot.slotAnchors[0]}; {newSlot.slotAnchors[1]}; {newSlot.slotAnchors[2]}; {newSlot.slotAnchors[3]}");
        }
    }

    private int CheckAnchors(Vector2Int newPos, WaySlot newSlot, WaySlot slotToConnect, int direction, int lastDirection)
    {
        int dir= 0;
        if (newSlot.slot.id == 2)
        {
            if (lastDirection == 0)
            {
                dir = (UnityEngine.Random.Range(0, 20) % 2 == 0) ? 1 : 3;
                
                newPos = slotToConnect.slot.pos + directions[dir];
                if(_spawnedSlots[newPos.x, newPos.y] != null)
                {
                    Destroy(newSlot);
                }
                else
                    newSlot.RotateSlot(Vector3.up, angles[dir]);
            }
            if (lastDirection == 0 && direction == 1)
            {
                dir = 0;
                newSlot.RotateSlot(Vector3.up, angles[dir]);
            }
            if (lastDirection == 1)
            {
                dir = (UnityEngine.Random.Range(0, 20) % 2 == 0) ? 1 : 3;
                newSlot.RotateSlot(Vector3.up, angles[dir]);
            }
            if (lastDirection == 1)
            {
                dir = (UnityEngine.Random.Range(0, 20) % 2 == 0) ? 1 : 3;
                newSlot.RotateSlot(Vector3.up, angles[dir]);
            }
        }
        return direction;
        for (int i = 0; i < 4; i++)
        {
            newSlot.RotateSlot(Vector3.up, angles[i]);

            Debug.Log($@"i:{i} Dir:{direction} RevDir:{(i + 2) % 4} Angle:{angles[i]}
            {direction}:{slotToConnect.slotAnchors[direction]} {(direction + 2) % 4}:{newSlot.slotAnchors[(direction + 2) % 4]}
            {newSlot.slotAnchors[0]}; {newSlot.slotAnchors[1]}; {newSlot.slotAnchors[2]}; {newSlot.slotAnchors[3]}");

            if (slotToConnect.slotAnchors[direction] && newSlot.slotAnchors[(direction + 2) % 4])
            {
                return i;
            }
        }
        //newSlot.RotateSlot(Vector3.up, angles[direction]);
        return direction;
        
    }

    private int RandomId()
    {
        int id = (UnityEngine.Random.Range(0, 20) < 7) ? 2 : 1;

        return id;
    }

    private void SetSlot(Vector2Int newPos, ref WaySlot newSlot, ref WaySlot slotToConnect, int newSlotAnchorId, int slotToConnectAnchorId)
    {
        newSlot.slot.pos = newPos;
        newSlot.transform.position = new Vector3(newPos.x, 0, newPos.y) * 30;

        newSlot.slotAnchors[newSlotAnchorId] = false;
        slotToConnect.slotAnchors[slotToConnectAnchorId] = false;

        _spawnedSlots[newPos.x, newPos.y] = newSlot;
    }

    /*
    private void PlaceOneEmptySlot()
    {
        for (int x = 0; x < _spawnedSlots.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedSlots.GetLength(1); y++)
            {
                if (_spawnedSlots[x, y] == null) continue;

                WaySlot newSlot = Instantiate(SlotPrefabs[0].GetComponent<WaySlot>());
                newSlot.transform.position = new Vector3(newSlot.pos.x, 0, newSlot.pos.y) * 30;
                _spawnedSlots[newSlot.pos.x, newSlot.pos.y] = newSlot;
            }
        }
        Vector2Int position = _vacantPlaces.ElementAt(UnityEngine.Random.Range(0, _vacantPlaces.Count));
    }
    */
    void Update()
    {

    }
}
