using UnityEngine;

[CreateAssetMenu(menuName = "Map/new slot", fileName = "New slot", order = 52)]
public class Slot : ScriptableObject
{
    [SerializeField] public int value;
    public Vector2Int _pos;

    public Vector2Int pos
    {
        get => _pos;
        set { _pos = value; }
    }
}
