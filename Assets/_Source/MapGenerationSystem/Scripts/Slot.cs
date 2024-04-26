using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Slot/new slot", fileName = "New slot", order = 52)]
public class Slot : ScriptableObject
{
    [SerializeField] public int id;
    private Vector2Int _pos;

    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }
}
